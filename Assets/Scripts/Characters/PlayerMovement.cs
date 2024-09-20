using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private GameManager _gameManager;
    public Tilemap tilemap;           // Tilemap, по которой будет двигаться персонаж
    public Vector3Int currentTile;    // Текущая клетка персонажа
    
    public float moveSpeed = 1f;      // Скорость движения (настраивается меню префаба)
    private Vector3 _targetPosition;   // Целевая позиция для перемещения
    private bool _isMoving;    // Флаг, что персонаж в движении

    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
        _targetPosition = transform.position;
        _gameManager = FindObjectOfType<GameManager>();
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
        }
    }

    private void HandlePlayerInput()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3Int targetTile = tilemap.WorldToCell(mousePos);

        if (targetTile == currentTile)
        {
            Debug.Log("Player is already on this tile.");
            return;
        }

        if (IsWithinOneTileRadius(targetTile) && TileExists(targetTile))
        {
            MoveToTile(targetTile);
        }
    }

    private bool IsWithinOneTileRadius(Vector3Int targetTile)
    {
        int dx = Mathf.Abs(targetTile.x - currentTile.x);
        int dy = Mathf.Abs(targetTile.y - currentTile.y);

        return dx <= 1 && dy <= 1;
    }

    private bool TileExists(Vector3Int targetTile)
    {
        TileBase tileBase = tilemap.GetTile(targetTile);
        return tileBase != null;
    }

    private void MoveToTile(Vector3Int targetTile)
    {
        if (!_isMoving)
        {
            _targetPosition = tilemap.GetCellCenterWorld(targetTile);
            currentTile = targetTile;
            _isMoving = true;
        }
    }
}
