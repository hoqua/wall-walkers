using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkeletonMovement : Enemy
{
    private Tilemap _tilemap;                          // Tilemap по которому будет двигаться враг
    private SkeletonAttack _skeletonAttack;                  // Ссылка на скрипт отвечающий за атаку врага
    private PlayerMovement _player;                    // Ссылка на скрипт игрока
    private SpawnManager _spawnManager;                // Ссылка для получения радиуса от игрока на котором враг может двигаться
   
    public Vector3Int currentTile;                     // Текущий тайл врага
    private bool _isActive = false;
    [SerializeField] private float moveSpeed = 5f;       // Скорость движения врага
    private Vector3 _targetPosition;                     // Целевая позиция для перемещения
    private bool _isMoving;                              // Флаг, что враг в движении
    
    void Start()
    {
        _tilemap = FindObjectOfType<Tilemap>();
        _skeletonAttack = GetComponent<SkeletonAttack>();
        _player = FindObjectOfType<PlayerMovement>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _targetPosition = transform.position;
        
        EnemyPositionManager.Instance.RegisterEnemy(currentTile);
    }

    public override void EnemyTurn()
    {
        MoveTowardsPlayer();
    }

    void OnDestroy()
    {
        EnemyPositionManager.Instance.UnregisterEnemy(currentTile);
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
        
        // Проверяем расстояние между врагом и игроком
        float distance = Vector3Int.Distance(currentTile, playerTile);
        if (distance > _spawnManager.spawnRadius - 3)
        {
            return; // Враг не двигается, если игрок вне радиуса
        }
        
        // Если игрок в зоне досягаемости, атакуем
        if (_skeletonAttack.IsPlayerInRange(currentTile, playerTile))
        {
            _skeletonAttack.AttackPlayer();
            return; // Если враг ударил игрока, он не двигается
        }

        Vector3Int direction = GetDirectionTowardsPlayer(playerTile);
        Vector3Int targetTile = currentTile + direction;

        // Проверяем, что тайл свободен и на нём нет другого врага
        if (_tilemap.GetTile(targetTile) != null && targetTile != playerTile && !IsTileOccupied(targetTile))
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
            EnemyPositionManager.Instance.UnregisterEnemy(currentTile);
            currentTile = targetTile;
            EnemyPositionManager.Instance.RegisterEnemy(currentTile);

            _isMoving = true; // Устанавливаем флаг, что враг в движении
        }
    }

    // Проверяем, занята ли клетка другим врагом
    private bool IsTileOccupied(Vector3Int tilePosition)
    {
        return EnemyPositionManager.Instance.IsTileOccupied(tilePosition) || _spawnManager.IsTileOccupiedByItem(tilePosition);
    }


}
