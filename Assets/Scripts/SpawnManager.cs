using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameObject = UnityEngine.GameObject;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Tilemap tilemap;                         // Tilemap для спавна персонажей
    private PlayerMovement _player;                                   // Ссылка на скрипт движения игрока для спавна объектов вокруг него
    
    [SerializeField] private GameObject playerPrefab;                 // Префаб игрока
    [SerializeField] private GameObject enemyPrefab;                  // Префаб врага
    
    [SerializeField] private GameObject expGemPrefab;                 // Префаб кристалла с опытом
    [SerializeField] private GameObject healthPotionPrefab;           // Префаб зелья с здоровьем
    
    [SerializeField] private GameObject expGemsContainer;             // Контейнер для хранения гемов 
    [SerializeField] private GameObject healthPotionContainer;        // Контейнер для зелий здоровья 
    
    [SerializeField] public float spawnRadius = 13f;                  // Радиус спавна от игрока
    private int _spawnDelay = 200;
    
    [SerializeField] [Range(0f, 1f)] private float objectSpawnChance = 0.55f; // Общий шанс появления объектов (врагов, кристаллов и тд.)
    [SerializeField] [Range(0f, 1f)] private float enemySpawnChance = 0.1f;   // Шанс появления врага
    [SerializeField] [Range(0f, 1f)] private float expSpawnChance = 0.85f;     // Шанс появления кристалла опыта
    [SerializeField] [Range(0f, 1f)] private float healthPotionSpawnChance = 0.05f;     // Шанс появления кристалла опыта
    
    private Vector3Int _playerSpawnPosition;                          // Место появления игрока
    private List<Vector3Int> _occupiedPositions = new List<Vector3Int>();
    
    private Dictionary<string, List<Vector3Int>> _itemPositions = new Dictionary<string, List<Vector3Int>>();
    
    async void Start()
    {
        await SpawnPlayer();             // Спавн игрока
        await Task.Delay(_spawnDelay);   // Задержка

        _player = FindObjectOfType<PlayerMovement>();

        SpawnObjectsAroundPlayer(_player.currentTile);
    }

    private async Task SpawnPlayer() 
    {
        _playerSpawnPosition = GetRandomSpawnPosition(); // Инициализация позиции для игрока
        
        var player = Instantiate(playerPrefab, tilemap.GetCellCenterWorld(_playerSpawnPosition), Quaternion.identity);
        player.GetComponent<PlayerMovement>().SetCurrentTile(_playerSpawnPosition, tilemap);
        
        _occupiedPositions.Add(_playerSpawnPosition);
        
        await Task.CompletedTask;
    }
    
    public void SpawnObjectsAroundPlayer(Vector3Int playerPosition)
    {
        List<Vector3Int> availablePositions = GetAvailablePositionsAroundPlayer(playerPosition);  // Получаем доступные позиции

        foreach (Vector3Int spawnPosition in availablePositions)
        {
            // Спавнить ли объект
            if (Random.value < objectSpawnChance) // Общий шанс появления
            {
                // Случайно выбирается, какой объект спавнить
                if (Random.value < enemySpawnChance)
                {
                    GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(spawnPosition),
                        Quaternion.identity);
                    EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
                    enemyScript.SetCurrentTile(spawnPosition, tilemap);
                    // Добавляем врага в список врагов
                    _gameManager.AddEnemy(enemyScript);
                }
                else if (Random.value < expSpawnChance)
                {
                    SpawnItem(expGemPrefab, expGemsContainer, spawnPosition, "ExpGem");
                }
                else if (Random.value < healthPotionSpawnChance)
                {
                    SpawnItem(healthPotionPrefab, healthPotionContainer, spawnPosition, "HealthPotion");
                }
            }
            _occupiedPositions.Add(spawnPosition);  // Добавляем в список занятых
        }
    }

    // Спавнит объекты на клетке, которую игрок покинул
    public void SpawnItemOnTile(Vector3Int tilePosition)
    {
        if (Random.value < 0.9f) // 90% шанс появления
        {
            SpawnItem(expGemPrefab, expGemsContainer, tilePosition, "ExpGem");
        }
    }

    private void SpawnItem(GameObject prefab, GameObject container, Vector3Int tilePosition, string itemType)
    {
        GameObject item = Instantiate(prefab, tilemap.GetCellCenterWorld(tilePosition), Quaternion.identity, container.transform);
        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 10);
        AddItemPosition(tilePosition, itemType);
    }
    
    // Метод для генерации случайной позиции на карте для спавна игрока
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
    
    // Получение всех доступных позиций вокруг игрока в пределах радиуса
    private List<Vector3Int> GetAvailablePositionsAroundPlayer(Vector3Int playerPosition)
    {
        List<Vector3Int> availablePositions = new List<Vector3Int>();

        // Перебираем все клетки в пределах радиуса
        for (int x = -Mathf.FloorToInt(spawnRadius); x <= Mathf.FloorToInt(spawnRadius); x++)
        {
            for (int y = -Mathf.FloorToInt(spawnRadius); y <= Mathf.FloorToInt(spawnRadius); y++)
            {
                Vector3Int offset = new Vector3Int(x, y, 0);
                Vector3Int potentialPosition = playerPosition + offset;

                // Проверяем, что клетка существует и не занята
                if (TileExists(potentialPosition) && !_occupiedPositions.Contains(potentialPosition))
                {
                    availablePositions.Add(potentialPosition);  // Добавляем клетку в список доступных
                }
            }
        }

        return availablePositions;
    }
    
    public void AddItemPosition(Vector3Int position, string itemType)
    {
        if (!_itemPositions.ContainsKey(itemType))
        {
            _itemPositions[itemType] = new List<Vector3Int>();
        }
        if (!_itemPositions[itemType].Contains(position))
        {
            _itemPositions[itemType].Add(position);
        }
    }

    public void RemoveItemPosition(Vector3Int position, string itemType)
    {
        if (_itemPositions.ContainsKey(itemType))
        {
            _itemPositions[itemType].Remove(position);
        }
    }
    
    public bool IsTileOccupiedByItem(Vector3Int position)
    {
        foreach (var itemList in _itemPositions.Values)
        {
            if (itemList.Contains(position))
            {
                return true;
            }
        }
        return false;
    }



    private bool TileExists(Vector3Int position) 
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null;
    }
    
}
