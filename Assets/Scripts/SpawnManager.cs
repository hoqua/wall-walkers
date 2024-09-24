using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour {
  public Tilemap tilemap;                  // Tilemap для спавна персонажей
  public Camera mainCamera;                // Камера

  public GameObject playerPrefab;          // Ссылка на Префаб игрока
  public GameObject enemyPrefab;           // Ссылка на Префаб врага

  private Vector3Int _playerSpawnPosition; // Место появления игрока
  private Vector3Int _enemySpawnPosition;  // Место появления врага

  void Start() {
    SpawnPlayer();
    SpawnEnemy();
  }

  

  private void SpawnPlayer() {
    _playerSpawnPosition = GetRandomSpawnPosition(); // Инициализация позиции для игрока
    
    var player = Instantiate(playerPrefab, tilemap.GetCellCenterWorld(GetRandomSpawnPosition()), Quaternion.identity);
    player.GetComponent<PlayerMovement>().SetCurrentTile(_playerSpawnPosition, tilemap);
    
    Debug.Log($"Player spawned at: {_playerSpawnPosition}");
  }
  
  private void SpawnEnemy()
  {
    _enemySpawnPosition = GetRandomSpawnPosition(); // Инициализация позиции для врага
    
    // Если позиции врага и игрока совпадают, то ищет другую позицию
    while (_enemySpawnPosition == _playerSpawnPosition)
    {
      _enemySpawnPosition = GetRandomSpawnPosition();
    }

    GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(_enemySpawnPosition), Quaternion.identity);
    enemy.GetComponent<EnemyMovement>().SetCurrentTile(_enemySpawnPosition, tilemap);
    
    Debug.Log($"Enemy spawned at: {_enemySpawnPosition}");;
  }

  private Vector3Int GetRandomSpawnPosition() {
    Vector3Int randomPosition;
    BoundsInt visibleBounds = GetVisibleTileBounds();

    do {
      int randomX = Random.Range(visibleBounds.xMin, visibleBounds.xMax);
      int randomY = Random.Range(visibleBounds.yMin, visibleBounds.yMax);
      randomPosition = new Vector3Int(randomX, randomY, 0);
    } while (!TileExists(randomPosition));

    return randomPosition;
  }

  private BoundsInt GetVisibleTileBounds() {
    Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -mainCamera.transform.position.z));
    Vector3 topRight =
      mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -mainCamera.transform.position.z));

    Vector3Int bottomLeftTile = tilemap.WorldToCell(bottomLeft);
    Vector3Int topRightTile = tilemap.WorldToCell(topRight);

    Debug.Log($"Visible bounds - BottomLeft: {bottomLeftTile}, TopRight: {topRightTile}");

    return new BoundsInt(bottomLeftTile, topRightTile - bottomLeftTile);
  }


  private bool TileExists(Vector3Int position) {
    TileBase tile = tilemap.GetTile(position);
    return tile != null;
  }
}