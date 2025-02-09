using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    private PlayerMovement _player;                                  // Ссылка на скрипт игрока
    private List<EnemyMovement> _enemies = new();                    // Список врагов
    
    [SerializeField] private ItemSelectScreen itemSelectScreen;                 // Экран для выбора предметов 
    private bool _isItemSelectionActive = false;
    
    private Camera _mainCamera;
    public GameState gameState = GameState.PlayerTurn;            // Начальное состояние игры
    private static event Action<GameState> OnGameStateChanged;      // Ивент для изменения состояния игры
    private async void Start()
    {
        await FindCharacters();
        await GameLoop();
    }

    // Находит игрока и всех врагов на сцене
    private async Task FindCharacters()
    {
        while (_player == null || _enemies.Count == 0) 
        {
            _player = FindObjectOfType<PlayerMovement>();
            _enemies.AddRange(FindObjectsOfType<EnemyMovement>()); // Находим всех врагов на сцене
            await Task.Yield();
        }
    }

    public void AddEnemy(EnemyMovement enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }
    
    private async Task GameLoop()
    {
        ChangeGameState(GameState.PlayerTurn);
        await Task.Delay(1000); // Задержка перед первым ходом

        // Игровой цикл
        while (gameState != GameState.GameOver)
        {
            await PlayerTurn();
            await EnemyTurn();
        }
    }

    private void ChangeGameState(GameState newState)
    {
        gameState = newState;
        OnGameStateChanged?.Invoke(gameState);
    }

    private async Task PlayerTurn()
    {
        _player.StartPlayersTurn();
        ChangeGameState(GameState.PlayerTurn);
        Debug.Log("Now: Player's Turn");

        // Ожидание хода игрока
        while (gameState == GameState.PlayerTurn)
        {
            if (_player.HasMovedOrAttacked())
            {
                break;
            }
            
            await Task.Yield();
        }
        
        Debug.Log("Player's Turn Ended");
        
        
        while (_isItemSelectionActive && itemSelectScreen != null)
        {
            itemSelectScreen.ShowItemSelectScreen();
            await Task.Yield();
        }

    }

    private async Task EnemyTurn()
    {
        // Ждём, пока окно выбора предметов не закроется
        while (_isItemSelectionActive)
        {
            await Task.Yield();
        }
        
        ChangeGameState(GameState.EnemyTurn);
        Debug.Log("Now: Enemy's Turn");

        await Task.Delay(90); // Задержка перед ходом врагов

        // Make a copy of the enemy list to avoid modification issues
        List<EnemyMovement> enemiesToMove = new List<EnemyMovement>(_enemies);
    
        foreach (var enemy in enemiesToMove)
        {
            enemy.MoveTowardsPlayer();
            await Task.Delay(25); // Задержка между ходами врагов
        }

        Debug.Log("Enemy's Turn Ended");
    }
    
    public void SetItemSelectionState(bool isActive)
    {
        _isItemSelectionActive = isActive;
    }

    public GameState CurrentState()
    {
        return gameState;
    }
}

public enum GameState
{
    PlayerTurn,
    EnemyTurn,
    SpawningPhase,
    EnvironmentTurn,
    GameOver
}
