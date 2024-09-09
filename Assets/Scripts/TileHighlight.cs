using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighlight : MonoBehaviour
{
    public Tilemap tilemap;               // Ссылка на Tilemap
    public Tile highlightTile;            // Тайл для выделения (созданный в tile palette)

    private Vector3Int previousTile;      // Последний выделенный тайл
    private TileBase previousTileBase;    // Сохраненный исходный тайл
    private bool hasPreviousTile = false; // Было ли выделение

    void Update()
    {
        HighlightTileUnderMouse();
    }
    
    void HighlightTileUnderMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        
        Vector3Int currentTile = tilemap.WorldToCell(mouseWorldPos);
        
        TileBase currentTileBase = tilemap.GetTile(currentTile);
        
        if (currentTileBase != null)
        {
            Vector3 cellCenter = tilemap.GetCellCenterWorld(currentTile);
            
            if (Vector3.Distance(mouseWorldPos, cellCenter) < tilemap.cellSize.x / 2)
            {
                if (currentTile != previousTile)
                {
                    if (hasPreviousTile)
                    {
                        tilemap.SetTile(previousTile, previousTileBase);
                    }
                    
                    previousTileBase = currentTileBase;
                    tilemap.SetTile(currentTile, highlightTile);
                    
                    previousTile = currentTile;
                    hasPreviousTile = true;
                }
            }
        }
        else
        {
            // Если мышь не над тайлом, сбрасываем выделение
            if (hasPreviousTile)
            {
                tilemap.SetTile(previousTile, previousTileBase);
                hasPreviousTile = false;
            }
        }
    }
}
