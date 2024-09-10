using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    public Tilemap tilemap;           // Tilemap, по которой будет двигаться персонаж
    public Vector3Int currentTile;     // Текущая клетка персонажа

    void Start()
    {
        transform.position = tilemap.GetCellCenterWorld(currentTile);
    }
    
    public void SetCurrentTile(Vector3Int tilePosition, Tilemap map)
    {
        currentTile = tilePosition;
        tilemap = map;
        transform.position = tilemap.GetCellCenterWorld(currentTile);

        Debug.Log($"Current Tile Set: {currentTile}, Position: {transform.position}");
    }

    public bool IsWithinOneTileRadius(Vector3Int targetTile)
    {
        int dx = Mathf.Abs(targetTile.x - currentTile.x);
        int dy = Mathf.Abs(targetTile.y - currentTile.y);

        Debug.Log($"Checking radius: Current Tile: {currentTile}, Target Tile: {targetTile}, dx: {dx}, dy: {dy}");

        return dx <= 1 && dy <= 1;
    }

    public bool TileExists(Vector3Int targetTile)
    {
        TileBase tileBase = tilemap.GetTile(targetTile);
        return tileBase != null;
    }

    public void MoveToTile(Vector3Int targetTile)
    {
        Vector3 worldPosition = tilemap.GetCellCenterWorld(targetTile);
        transform.position = worldPosition;
        currentTile = targetTile;

        Debug.Log($"Moved to Tile: {targetTile}, New Position: {transform.position}");
    }
}