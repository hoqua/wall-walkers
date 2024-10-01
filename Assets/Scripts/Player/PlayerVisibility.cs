using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerVisibility : MonoBehaviour
{
    public Tilemap tilemap;               // Ссылка на Tilemap
    public float visibilityRadiusY = 4f;  // Ограничение видимости по Y оси
    public LayerMask enemyLayer;          // Слой врагов (чтобы скрыть их)

    private Transform _player;            // Ссылка на игрока
    private Vector3Int _playerTile;  

    void Start()
    {
        FindPlayer(); // Ищем игрока при старте
    }

    void Update()
    {
        if (_player == null)
        {
            FindPlayer(); 
            return;
        }

        _playerTile = tilemap.WorldToCell(_player.position);
        UpdateTileVisibility();
        UpdateEnemyVisibility();
    }

    private void FindPlayer()
    {
        var playerObject = GameObject.FindWithTag("Player"); 
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
    }

    private void UpdateTileVisibility()
    {
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            
            float distanceY = Mathf.Abs(tilemap.GetCellCenterWorld(position).y - _player.position.y);
            
            if (distanceY <= visibilityRadiusY)
            {
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, Color.white);
            }
            else
            {
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, new Color(1, 1, 1, 0)); 
            }
        }
    }

    private void UpdateEnemyVisibility()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_player.position, visibilityRadiusY, enemyLayer);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distanceY = Mathf.Abs(_player.position.y - enemy.transform.position.y);
            
            if (distanceY <= visibilityRadiusY)
            {
                SetVisibility(enemy, true);
            }
            else
            {
                SetVisibility(enemy, false);
            }
        }
    }
    
    // Метод для управления видимостью врага и его дочерних объектов
    private void SetVisibility(GameObject enemy, bool isVisible)
    {
        enemy.GetComponent<SpriteRenderer>().enabled = isVisible;
        
        foreach (SpriteRenderer sr in enemy.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = isVisible;
        }
    }
}
