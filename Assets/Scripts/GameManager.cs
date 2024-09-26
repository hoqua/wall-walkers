using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public PlayerMovement player; // Ссылка на скрипт игрока
   public EnemyMovement enemy;   // Ссылка на скрипт врага

   private Camera _mainCamera;
   private GameState _gameState = GameState.PlayerTurn;        // Начальное состояние игры
   public static event Action<GameState> OnGameStateChanged;  // Ивент, отвечает за изменение состояния игры 

   private async void Start()
   {
      await FindCharacters();
      await GameLoop();
   }

   private async Task FindCharacters()
   {
      while (player == null || enemy == null) // Используем "или", чтобы проверять оба объекта
      {
         player = FindObjectOfType<PlayerMovement>();
         enemy = FindObjectOfType<EnemyMovement>();
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

      await Task.Delay(500); // Задержка перед ходом врага (как будто думает)
      enemy.MoveTowardsPlayer();

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
