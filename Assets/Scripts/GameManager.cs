using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
  public Movement player; //Ссылка на скрипт игрока
  public EnemyMovement enemy; // Ссылка на скрипт врага

  private Camera _mainCamera;
  private GameState _gameState = GameState.PlayerTurn;
  public static event Action<GameState> OnGameStateChanged;


  private void Awake() {
    var spawner = FindObjectOfType<SpawnManager>();
    spawner.SpawnPlayer();
    _mainCamera = Camera.main;
  }
}

public enum GameState {
  PlayerTurn,
  EnemyTurn,
  EnvironmentTurn,
  SpawningPhase,
  GameOver
}