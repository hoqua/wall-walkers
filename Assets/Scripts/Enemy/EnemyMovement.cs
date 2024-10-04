using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    private Tilemap _tilemap;                          // Tilemap по которому будет двигаться враг
    public Vector3Int currentTile;                     // Текущий тайл врага
    private EnemyAttack _enemyAttack;                  // Ссылка на скрипт отвечающий за атаку врага
    private PlayerMovement _player;                    // Ссылка на скрипт игрока

    private bool _isActive = false;
    [SerializeField] private float moveSpeed = 5f;     // Скорость движения врага
    private Vector3 _targetPosition;                   // Целевая позиция для перемещения
    private bool _isMoving;                            // Флаг, что враг в движении

    // Статический список для отслеживания всех врагов и их позиций
    private static List<Vector3Int> _enemyPositions = new List<Vector3Int>();

    void Start()
    {
        _tilemap = FindObjectOfType<Tilemap>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _player = FindObjectOfType<PlayerMovement>();
        _targetPosition = transform.position;
        
        _enemyPositions.Add(currentTile);
    }

    void OnDestroy()
    {
        _enemyPositions.Remove(currentTile);
    }

    public void SetCurrentTile(Vector3Int tilePosition, Tilemap map)
    {
        currentTile = tilePosition;
        _tilemap = map;
        transform.position = _tilemap.GetCellCenterWorld(currentTile);
        _targetPosition = transform.position;
    }

    void Update()
    {
        if (_isMoving)
        {
            MoveTowardsTarget();
        } 
        else if (_isActive == false) 
        {
            MoveTowardsTarget();
            _isActive = true;
        }
    }

    public void MoveTowardsPlayer()
    {
        Vector3Int playerTile = _player.currentTile;

        // Если игрок в зоне досягаемости, атакуем
        if (_enemyAttack.IsPlayerInRange(currentTile, playerTile))
        {
            _enemyAttack.AttackPlayer();
            return; // Если враг ударил игрока, он не двигается
        }

        Vector3Int direction = GetDirectionTowardsPlayer(playerTile);
        Vector3Int targetTile = currentTile + direction;

        // Проверяем, что тайл свободен и на нём нет другого врага
        if (_tilemap.GetTile(targetTile) != null && targetTile != playerTile && !IsTileOccupiedByEnemy(targetTile))
        {
            MoveToTile(targetTile);
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
        {
            _isMoving = false;
        }
    }

    private Vector3Int GetDirectionTowardsPlayer(Vector3Int playerTile)
    {
        int dx = playerTile.x - currentTile.x;
        int dy = playerTile.y - currentTile.y;

        Vector3Int direction = new Vector3Int(
            dx != 0 ? Mathf.Clamp(dx, -1, 1) : 0,
            dy != 0 ? Mathf.Clamp(dy, -1, 1) : 0,
            0
        );
        return direction;
    }

    private void MoveToTile(Vector3Int targetTile)
    {
        if (!_isMoving)
        {
            _targetPosition = _tilemap.GetCellCenterWorld(targetTile);
            _enemyPositions.Remove(currentTile);  // Убираем старую позицию
            currentTile = targetTile;
            _enemyPositions.Add(currentTile);     // Добавляем новую позицию

            _isMoving = true; // Устанавливаем флаг, что враг в движении
        }
    }

    // Проверяем, занята ли клетка другим врагом
    private bool IsTileOccupiedByEnemy(Vector3Int tilePosition)
    {
        return _enemyPositions.Contains(tilePosition);
    }
}
