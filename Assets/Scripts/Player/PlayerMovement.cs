using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private PlayerAttack _playerAttackScript;
    private PlayerStats _playerStats;
    private Tilemap _tilemap;                 // Tilemap, по которой будет двигаться персонаж

    private Vector3Int _lastTile;             // Предыдущая клетка игрока
    public Vector3Int currentTile;            // Текущая клетка игрока
    
    public float moveSpeed = 5f;              // Скорость движения (настраивается в меню префаба)
    private Vector3 _targetPosition;          // Целевая позиция для перемещения
    private bool _isMoving;                   // Флаг, что персонаж в движении
    public bool hasMoved;                     // Флаг, что персонаж походил и может начинаться ход врагов

    void Start()
    {
        _tilemap = FindObjectOfType<Tilemap>();
        _targetPosition = transform.position;
        _gameManager = FindObjectOfType<GameManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();

        _playerStats = GetComponent<PlayerStats>();
        _playerAttackScript = GetComponent<PlayerAttack>();

        if (currentTile == Vector3Int.zero)
        {
            Debug.Log($"Spawning item on tile: {_lastTile}");
            currentTile = _tilemap.WorldToCell(transform.position);
        }
    }
    
    public void SetCurrentTile(Vector3Int newTile, Tilemap tilemap)
    {
        _tilemap = tilemap;
        
        if (newTile != currentTile) 
        {
            _lastTile = currentTile;
            currentTile = newTile;  

            if (_lastTile != Vector3Int.zero) 
            {
                _spawnManager.SpawnItemOnTile(_lastTile); 
            }
        }
    }


    void Update()
    {
        if (_isMoving)
        {
            MoveTowardsTarget();
        } 
        else if (Input.GetMouseButtonDown(0))
        {
            HandlePlayerInput();
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
        {
            _isMoving = false;
            hasMoved = true;
            
            // Спавним объекты после перемещения
            _spawnManager.SpawnObjectsAroundPlayer(currentTile);
        }
    }

    private void HandlePlayerInput()
    {
        if (_gameManager.CurrentState() != GameState.PlayerTurn)
        {
            return; // Игрок не может двигаться, не его ход
        }
        
        Vector3Int targetTile = GetTargetTileFromMouse();

        if (targetTile == currentTile)
        {
            Debug.Log("Player is already on this tile.");
            return;
        }

        if (IsWithinOneTileRadius(targetTile) && TileExists(targetTile))
        {
            AttemptToMoveOrAttack(targetTile);
        }
    }

    private Vector3Int GetTargetTileFromMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return _tilemap.WorldToCell(mousePos);
    }

    private void AttemptToMoveOrAttack(Vector3Int targetTile)
    {
        GameObject targetObject = GetObjectOnTile(targetTile);

        // Если на клетке гем, то получаем опыт и удаляем гем
        if (targetObject != null && targetObject.CompareTag("ExpGem"))
        {
            _playerStats.GainExp();
            Destroy(targetObject);
        }
        
        // Передаем целевую клетку для атаки
        _playerAttackScript.HandleAttack(targetTile);

        // Если атаки не было, перемещаемся на клетку
        if (!_playerAttackScript.hasAttacked)
        {
            MoveToTile(targetTile);
        }
    }

    private GameObject GetObjectOnTile(Vector3Int targetTile)
    {
        Vector3 worldPosition = _tilemap.GetCellCenterWorld(targetTile);
        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("ExpGem"))
            {
                return collider.gameObject;
            }
        }

        return null;
    }

    private bool IsWithinOneTileRadius(Vector3Int targetTile)
    {
        int dx = Mathf.Abs(targetTile.x - currentTile.x);
        int dy = Mathf.Abs(targetTile.y - currentTile.y);

        return dx <= 1 && dy <= 1;
    }

    private bool TileExists(Vector3Int targetTile)
    {
        TileBase tileBase = _tilemap.GetTile(targetTile);
        return tileBase != null;
    }

    public void MoveToTile(Vector3Int targetTile)
    {
        if (!_isMoving)
        {
            _targetPosition = _tilemap.GetCellCenterWorld(targetTile);
            SetCurrentTile(targetTile, _tilemap);
            currentTile = targetTile;
            _isMoving = true;
            hasMoved = false;
        }
    }

    public void StartPlayersTurn()
    {
        hasMoved = false;
        _playerAttackScript.ResetAttack();
    }
    
    public bool HasMovedOrAttacked()
    {
        return hasMoved || _playerAttackScript.hasAttacked;
    }
}
