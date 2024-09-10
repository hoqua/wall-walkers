
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap по которому будет двигаться враг
    public Vector3Int currentTile; // Текущий тайл врага
    public CharacterMovement player;// Ссылка на скрипт игрока

    void Start()
    {
        transform.position = tilemap.GetCellCenterWorld(currentTile);
    }

    public void MoveTowardsPlayer()
    {
        Vector3Int playerTile = player.currentTile;
        Vector3Int direction = GetDirectionTowardsPlayer(playerTile);

        Vector3Int targetTile = currentTile + direction;

        if (tilemap.GetTile(targetTile) != null && targetTile != playerTile)
        {
            MoveToTile(targetTile);
        }
    }

    
    private Vector3Int GetDirectionTowardsPlayer(Vector3Int playerTile)
    {
        int dx = playerTile.x - currentTile.x;
        int dy = playerTile.y - currentTile.y;

        Vector3Int direction = new Vector3Int(
            dx != 0 ? Mathf.Clamp(dx, -1, 1) : 0,
            dy != 0 ? Mathf.Clamp(dy, -1, 1) : 0,
            0
        );
        return direction;
    }
    
    private void MoveToTile(Vector3Int targetTile)
    {
        Vector3 worldPosition = tilemap.GetCellCenterWorld(targetTile);
        transform.position = worldPosition;
        currentTile = targetTile;
        
        Debug.Log($"Enemy moved to Tile: {targetTile}, New Position: {transform.position}");
    }
}
