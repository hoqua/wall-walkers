using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private PlayerMovement _player;                                  // Ссылка на скрипт игрока
    private List<EnemyMovement> _enemies = new();                    // Список врагов

    private Camera _mainCamera;
    private GameState _gameState = GameState.PlayerTurn;            // Начальное состояние игры
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

    private async Task GameLoop()
    {
        ChangeGameState(GameState.PlayerTurn);
        await Task.Delay(1000); // Задержка перед первым ходом

        // Игровой цикл
        while (_gameState != GameState.GameOver)
        {
            await PlayerTurn();
            await EnemyTurn();
        }
    }

    private void ChangeGameState(GameState newState)
    {
        _gameState = newState;
        OnGameStateChanged?.Invoke(_gameState);
    }

    private async Task PlayerTurn()
    {
        _player.StartPlayersTurn();
        ChangeGameState(GameState.PlayerTurn);
        Debug.Log("Now: Player's Turn");

        // Ожидание хода игрока
        while (_gameState == GameState.PlayerTurn)
        {
            if (_player.HasMovedOrAttacked())
            {
                break;
            }
            await Task.Yield();
        }

        Debug.Log("Player's Turn Ended");
    }

    private async Task EnemyTurn()
    {
        ChangeGameState(GameState.EnemyTurn);
        Debug.Log("Now: Enemy's Turn");

        await Task.Delay(500); // Задержка перед ходом врагов
        
        foreach (var enemy in _enemies)
        {
            enemy.MoveTowardsPlayer();
            await Task.Delay(50); // Задержка между ходами врагов
        }

        Debug.Log("Enemy's Turn Ended");
    }

    public GameState CurrentState()
    {
        return _gameState;
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
