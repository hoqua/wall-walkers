using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap;             // Tilemap по которому будет двигаться враг
    public Vector3Int currentTile;      // Текущий тайл врага
    private EnemyAttack _enemyAttack;   // Ссылка на скрипт отвечающий за атаку врага
    public PlayerMovement player;       // Ссылка на скрипт игрока

    public float moveSpeed = 5f;        // Скорость движения игрока
    private Vector3 _targetPosition;    // Целевая позиция для перемещения
    private bool _isMoving;             // Флаг, что враг в движении
    
    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
        _enemyAttack = GetComponent<EnemyAttack>();
        player = FindObjectOfType<PlayerMovement>();
        _targetPosition = transform.position;
    }

    public void SetCurrentTile(Vector3Int tilePosition, Tilemap map)
    {
        currentTile = tilePosition;
        tilemap = map;
        transform.position = tilemap.GetCellCenterWorld(currentTile);
        _targetPosition = transform.position;
    }

    void Update()
    {
        if (_isMoving)
        {
            MoveTowardsTarget();
        }
    }
    
    public void MoveTowardsPlayer()
    {
        Vector3Int playerTile = player.currentTile;

        if (_enemyAttack.IsPlayerInRange(currentTile, playerTile))
        {
            _enemyAttack.AttackPlayer();
            return; // Если враг ударил врага, то он не двигается
        }
        
        Vector3Int direction = GetDirectionTowardsPlayer(playerTile);
        Vector3Int targetTile = currentTile + direction;

        if (tilemap.GetTile(targetTile) != null && targetTile != playerTile)
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
            _targetPosition = tilemap.GetCellCenterWorld(targetTile);
            currentTile = targetTile;

            _isMoving = true; // Устанавливаем флаг, что враг в движении
        }
    }
}
