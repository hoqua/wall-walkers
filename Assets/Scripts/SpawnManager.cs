using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour {
  

  [SerializeField] private Tilemap tilemap;                         // Tilemap для спавна персонажей

  [SerializeField] private GameObject playerPrefab;     // Ссылка на Префаб игрока (задается в Unity)
  [SerializeField] private GameObject enemyPrefab;      // Ссылка на Префаб врага (задается в Unity)
  [SerializeField] private GameObject expGemPrefab;     // Ссылка на Префаб камня с опытом (задается в Unity)
  [SerializeField] private GameObject expGemsContainer; // Контейнер для хранения гемов 
  [SerializeField] private int enemyCount = 3;          // Количество врагов для спавна (задается в Unity)
  private int _spawnDelay = 200;
  
  private Vector3Int _playerSpawnPosition;          // Место появления игрока
  private List<Vector3Int> _enemySpawnPositions = new List<Vector3Int>(); // Список мест появления врагов
  private List<Vector3Int> _occupiedPositions = new List<Vector3Int>();

  async void Start()
  {
    await SpawnPlayer();             // Спавн игрока
    await Task.Delay(_spawnDelay);   // Задержка
    
    await SpawnEnemies(enemyCount);  // Спавн нескольких врагов
    await Task.Delay(_spawnDelay);   // Задержка
    
    await SpawnExpGems();            // Спавн гемов с опытом
  }

  

  private async Task SpawnPlayer() 
  {
    _playerSpawnPosition = GetRandomSpawnPosition(); // Инициализация позиции для игрока
    
    var player = Instantiate(playerPrefab, tilemap.GetCellCenterWorld(_playerSpawnPosition), Quaternion.identity);
    player.GetComponent<PlayerMovement>().SetCurrentTile(_playerSpawnPosition, tilemap);
    
    _occupiedPositions.Add(_playerSpawnPosition);
    
    Debug.Log($"Player spawned at: {_playerSpawnPosition}");
    
    await Task.CompletedTask;
  }
  
  private async Task SpawnEnemies(int count) 
  {
    for (int i = 0; i < count; i++) {
      Vector3Int enemySpawnPosition = GetRandomSpawnPosition();
      
      while (_occupiedPositions.Contains(enemySpawnPosition)) {
        enemySpawnPosition = GetRandomSpawnPosition();
      }

      _enemySpawnPositions.Add(enemySpawnPosition);  
      _occupiedPositions.Add(enemySpawnPosition); 
      
      GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(enemySpawnPosition), Quaternion.identity);
      enemy.GetComponent<EnemyMovement>().SetCurrentTile(enemySpawnPosition, tilemap);
    
      Debug.Log($"Enemy {i + 1} spawned at: {enemySpawnPosition}");
      await Task.CompletedTask;
    }
  }

  private async Task SpawnExpGems()
  {
    BoundsInt bounds = tilemap.cellBounds;
    TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

    List<Vector3Int> gemPositions = new List<Vector3Int>();
    
    for (int x = bounds.xMin; x < bounds.xMax; x++)
    {
      for (int y = bounds.yMin; y < bounds.yMax; y++)
      {
        Vector3Int tilePosition = new Vector3Int(x, y, 0);

        if (!_occupiedPositions.Contains(tilePosition) && TileExists(tilePosition))
        {
          gemPositions.Add(tilePosition);
        }
      }
    }
    
    foreach (Vector3Int gemPosition in gemPositions)
    {
      Vector3 worldPosition = tilemap.GetCellCenterWorld(gemPosition);  
      var gem = Instantiate(expGemPrefab, worldPosition, Quaternion.identity, expGemsContainer.transform);     
      _occupiedPositions.Add(gemPosition);        
      
      await Task.Delay(1); // Задержка перед спавном КАЖДОГО гема (0 это минимальная задержка)
    }

    Debug.Log($"{gemPositions.Count} Exp Gems spawned.");
    await Task.CompletedTask;
  }


  private Vector3Int GetRandomSpawnPosition()
  {
    BoundsInt bounds = tilemap.cellBounds;
    Vector3Int randomPosition;

    do
    {
      int randomX = Random.Range(bounds.xMin, bounds.xMax);
      int randomY = Random.Range(bounds.yMin, bounds.yMax);
      randomPosition = new Vector3Int(randomX, randomY, 0);
    } 
    while (!TileExists(randomPosition) || _occupiedPositions.Contains(randomPosition));

    return randomPosition;
  }

  private bool TileExists(Vector3Int position) {
    TileBase tile = tilemap.GetTile(position);
    return tile != null;
  }
}
