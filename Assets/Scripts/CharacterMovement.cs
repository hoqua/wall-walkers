using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement : MonoBehaviour
{
    private Tilemap tilemap;           // Tilemap, по которой будет двигаться персонаж
    public Vector3Int currentTile;     // Текущая клетка персонажа

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            
            Vector3Int targetTile = tilemap.WorldToCell(mousePos);

            Debug.Log($"Mouse Position: {mousePos}, Target Tile: {targetTile}");

            if (IsWithinOneTileRadius(targetTile))
            {
                MoveToTile(targetTile);
            }
            else
            {
                Debug.Log("Target tile is out of range.");
            }
        }
    }

    public void SetCurrentTile(Vector3Int tilePosition, Tilemap map)
    {
        currentTile = tilePosition;
        tilemap = map;
        transform.position = tilemap.GetCellCenterWorld(currentTile);

        Debug.Log($"Current Tile Set: {currentTile}, Position: {transform.position}");
    }

    bool IsWithinOneTileRadius(Vector3Int targetTile)
    {
        int dx = Mathf.Abs(targetTile.x - currentTile.x);
        int dy = Mathf.Abs(targetTile.y - currentTile.y);

        Debug.Log($"Checking radius: Current Tile: {currentTile}, Target Tile: {targetTile}, dx: {dx}, dy: {dy}");

        return dx <= 1 && dy <= 1;
    }

    void MoveToTile(Vector3Int targetTile)
    {
        Vector3 worldPosition = tilemap.GetCellCenterWorld(targetTile);
        transform.position = worldPosition;
        currentTile = targetTile;

        Debug.Log($"Moved to Tile: {targetTile}, New Position: {transform.position}");
    }
}
