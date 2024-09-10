using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap для спавна персонажей
    public Camera mainCamera;
   
    public GameObject playerPrefab;
    public GameObject enemyPrefab; 
   
    private Vector3Int _playerSpawnPosition;
    private Vector3Int _enemySpawnPosition;

    void Start()
    {
        _playerSpawnPosition = GetRandomSpawnPosition();
        _enemySpawnPosition = GetRandomSpawnPosition();

        // Убедиться, что позиции игрока и врага не совпадают
        while (_enemySpawnPosition == _playerSpawnPosition)
        {
            _enemySpawnPosition = GetRandomSpawnPosition();
        }

        // Спавним игрока
        GameObject player = Instantiate(playerPrefab, tilemap.GetCellCenterWorld(_playerSpawnPosition), Quaternion.identity);
        player.GetComponent<PlayerMovement>().SetCurrentTile(_playerSpawnPosition, tilemap);

        // Спавним врага
        GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(_enemySpawnPosition), Quaternion.identity);
        enemy.GetComponent<EnemyMovement>().SetCurrentTile(_enemySpawnPosition, tilemap);
      
        Debug.Log($"Player spawned at: {_playerSpawnPosition}, Enemy spawned at: {_enemySpawnPosition}");
    }

    private Vector3Int GetRandomSpawnPosition()
    {
        Vector3Int randomPosition;
        BoundsInt visibleBounds = GetVisibleTileBounds();

        // Находим случайную позицию в пределах видимой области, где есть тайл
        do
        {
            int randomX = Random.Range(visibleBounds.xMin, visibleBounds.xMax);
            int randomY = Random.Range(visibleBounds.yMin, visibleBounds.yMax);
            randomPosition = new Vector3Int(randomX, randomY, 0);

        } while (!TileExists(randomPosition));

        return randomPosition;
    }

    // Метод для вычисления границ видимой области тайлов в 2D
    private BoundsInt GetVisibleTileBounds()
    {
        // Получаем углы видимой области камеры с учётом того, что камера на -10 по Z
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -mainCamera.transform.position.z));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -mainCamera.transform.position.z));

        // Преобразуем мировые координаты в клеточные координаты тайлмапа
        Vector3Int bottomLeftTile = tilemap.WorldToCell(bottomLeft);
        Vector3Int topRightTile = tilemap.WorldToCell(topRight);

        Debug.Log($"Visible bounds - BottomLeft: {bottomLeftTile}, TopRight: {topRightTile}");

        // Создаем границы видимых тайлов
        return new BoundsInt(bottomLeftTile, topRightTile - bottomLeftTile);
    }
   
    
    private bool TileExists(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null;
    }
}
