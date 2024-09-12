using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  public static readonly string tag = "Player";
  public static event Action OnPlayerTurnEnded;


  private bool isPlayerTurn = true;
  private Movement movement;

  private void Awake() {
    gameObject.tag = tag;
    GameManager.OnGameStateChanged += OnGameStateChanged;
    movement = GetComponent<Movement>();
  }


  private async void Update() {
    if (!isPlayerTurn) return;
    if (!Input.GetMouseButtonDown(0)) return;


    movement.MoveToTile();

    OnPlayerTurnEnded?.Invoke();
  }

  private void OnDestroy() {
    GameManager.OnGameStateChanged -= OnGameStateChanged;
  }


  private void OnGameStateChanged(GameState gameState) {
    isPlayerTurn = gameState == GameState.PlayerTurn;
  }
}