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
    [SerializeField] private GameObject expGemPrefab;                 // Префаб камня с опытом
    [SerializeField] private GameObject expGemsContainer;             // Контейнер для хранения гемов 
    [SerializeField] private float spawnRadius = 13f;                 // Радиус спавна от игрока
    private int _spawnDelay = 200;
    
    private Vector3Int _playerSpawnPosition;                          // Место появления игрока
    private List<Vector3Int> _occupiedPositions = new List<Vector3Int>();

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
        
        Debug.Log($"Player spawned at: {_playerSpawnPosition}");
        
        await Task.CompletedTask;
    }
    
    public void SpawnObjectsAroundPlayer(Vector3Int playerPosition)
    {
        List<Vector3Int> availablePositions = GetAvailablePositionsAroundPlayer(playerPosition);  // Получаем доступные позиции

        foreach (Vector3Int spawnPosition in availablePositions)
        {
            // Решаем, какой объект спавнить (гем или враг)
            if (Random.value < 0.025f)
            {
                GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);
                EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
                enemyScript.SetCurrentTile(spawnPosition, tilemap);
                // Добавляем врага в список врагов
                _gameManager.AddEnemy(enemyScript);
            }
            else
            {
                GameObject expGem = Instantiate(expGemPrefab, tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity, expGemsContainer.transform);
                expGem.transform.position = new Vector3(expGem.transform.position.x, expGem.transform.position.y, 10);
            }

            _occupiedPositions.Add(spawnPosition);  // Добавляем в список занятых
            Debug.Log($"Spawned object at: {spawnPosition}");
        }
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

    private bool TileExists(Vector3Int position) 
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null;
    }
    
}
