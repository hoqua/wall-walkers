using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour {
  

  [SerializeField] private Tilemap tilemap;                         // Tilemap для спавна персонажей
  [SerializeField] private Camera mainCamera;                       // Камера

  [SerializeField] private GameObject playerPrefab; // Ссылка на Префаб игрока (задается в Unity)
  [SerializeField] private GameObject enemyPrefab;  // Ссылка на Префаб врага (задается в Unity)
  [SerializeField] private int enemyCount = 3;      // Количество врагов для спавна (задается в Unity)

  private Vector3Int _playerSpawnPosition;          // Место появления игрока
  private List<Vector3Int> _enemySpawnPositions = new List<Vector3Int>(); // Список мест появления врагов

  void Start()
  {
    SpawnPlayer();           
    SpawnEnemies(enemyCount);  // Спавн нескольких врагов
  }

  private void SpawnPlayer() {
    _playerSpawnPosition = GetRandomSpawnPosition(); // Инициализация позиции для игрока
    
    var player = Instantiate(playerPrefab, tilemap.GetCellCenterWorld(_playerSpawnPosition), Quaternion.identity);
    player.GetComponent<PlayerMovement>().SetCurrentTile(_playerSpawnPosition, tilemap);
    
    Debug.Log($"Player spawned at: {_playerSpawnPosition}");
  }

  private void SpawnEnemies(int count) {
    for (int i = 0; i < count; i++) {
      Vector3Int enemySpawnPosition = GetRandomSpawnPosition();
      
      // Проверяем, что позиция врага не совпадает с позицией игрока или другого врага
      while (_enemySpawnPositions.Contains(enemySpawnPosition) || enemySpawnPosition == _playerSpawnPosition) {
        enemySpawnPosition = GetRandomSpawnPosition();
      }

      _enemySpawnPositions.Add(enemySpawnPosition);  // Добавляем позицию в список врагов

      GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(enemySpawnPosition), Quaternion.identity);
      enemy.GetComponent<EnemyMovement>().SetCurrentTile(enemySpawnPosition, tilemap);
    
      Debug.Log($"Enemy {i + 1} spawned at: {enemySpawnPosition}");
    }
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
    Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -mainCamera.transform.position.z));

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
