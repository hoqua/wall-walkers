using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;                  // Ссылка на скрипт игрока
    public List<EnemyMovement> enemies = new List<EnemyMovement>(); // Список врагов

    private Camera _mainCamera;
    private GameState _gameState = GameState.PlayerTurn; // Начальное состояние игры
    public static event Action<GameState> OnGameStateChanged; // Ивент для изменения состояния игры

    private async void Start()
    {
        await FindCharacters();
        await GameLoop();
    }

    // Находит игрока и всех врагов на сцене
    private async Task FindCharacters()
    {
        while (player == null || enemies.Count == 0) // Используем список врагов
        {
            player = FindObjectOfType<PlayerMovement>();
            enemies.AddRange(FindObjectsOfType<EnemyMovement>()); // Находим всех врагов на сцене
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
        player.StartPlayersTurn();
        ChangeGameState(GameState.PlayerTurn);
        Debug.Log("Now: Player's Turn");

        // Ожидание хода игрока
        while (_gameState == GameState.PlayerTurn)
        {
            if (player.HasMovedOrAttacked())
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

        // Проходим по каждому врагу и даем ему команду на движение
        foreach (var enemy in enemies)
        {
            enemy.MoveTowardsPlayer();
            await Task.Delay(500); // Задержка между ходами врагов, если нужно
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
