using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap tilemap;           // Tilemap, по которой будет двигаться персонаж
    public Vector3Int currentTile;    // Текущая клетка персонажа
    public float moveSpeed = 1f;      // Скорость перемещения

    private Vector3 _targetPosition;   // Целевая позиция для перемещения
    private bool _isMoving;    // Флаг, что персонаж в движении

    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
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
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
            {
                _isMoving = false;
            }
        }
    }

    public bool IsWithinOneTileRadius(Vector3Int targetTile)
    {
        int dx = Mathf.Abs(targetTile.x - currentTile.x);
        int dy = Mathf.Abs(targetTile.y - currentTile.y);

        return dx <= 1 && dy <= 1;
    }

    public bool TileExists(Vector3Int targetTile)
    {
        TileBase tileBase = tilemap.GetTile(targetTile);
        return tileBase != null;
    }

    public void MoveToTile(Vector3Int targetTile)
    {
        if (!_isMoving)
        {
            _targetPosition = tilemap.GetCellCenterWorld(targetTile);
            currentTile = targetTile;
            _isMoving = true;
        }
    }
}
