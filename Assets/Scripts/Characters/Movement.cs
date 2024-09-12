using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour {
  public Tilemap tilemap; // Tilemap, по которой будет двигаться персонаж
  public Vector3Int currentTile; // Текущая клетка персонажа

  private void Awake() {
    tilemap = FindObjectOfType<Tilemap>();
  }

  public void SetCurrentTile(Vector3Int tilePosition, Tilemap map) {
    currentTile = tilePosition;
    tilemap = map;
    transform.position = tilemap.GetCellCenterWorld(currentTile);
  }

  public bool IsWithinOneTileRadius(Vector3Int targetTile) {
    int dx = Mathf.Abs(targetTile.x - currentTile.x);
    int dy = Mathf.Abs(targetTile.y - currentTile.y);

    return dx <= 1 && dy <= 1;
  }

  public bool TileExists(Vector3Int targetTile) {
    return tilemap.GetTile(targetTile) != null;
  }

  public void MoveToTile() {
    var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    clickPosition.z = 0f;
    var cellPosition = tilemap.WorldToCell(clickPosition);
    var targetPosition = tilemap.GetCellCenterWorld(cellPosition);
    transform.position = targetPosition;
  }
}