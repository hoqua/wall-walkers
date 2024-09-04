using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricTileHoverOverlay : MonoBehaviour
{
    public Tilemap tilemap; 
    public GameObject overlayPrefab; 

    private GameObject _currentOverlay; 

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; 
        
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
        
        for (int z = -5; z <= 5; z++)
        {
            Vector3Int checkPos = new Vector3Int(cellPosition.x, cellPosition.y, z);
            if (tilemap.HasTile(checkPos))
            {
                Vector3 tileWorldPosition = tilemap.GetCellCenterWorld(checkPos);
                tileWorldPosition.z = checkPos.z;
                
                Vector3 adjustedPosition = AdjustOverlayPosition(tileWorldPosition, checkPos.z);
                
                if (_currentOverlay != null)
                {
                    _currentOverlay.transform.position = adjustedPosition;
                }
                else
                {
                    _currentOverlay = Instantiate(overlayPrefab, adjustedPosition, Quaternion.identity);
                }
                return;
            }
        }
        
        if (_currentOverlay != null)
        {
            Destroy(_currentOverlay);
        }
    }
    
    private Vector3 AdjustOverlayPosition(Vector3 position, int z)
    {
        
        float zOffset = -z * 0.1f; // Смещение спрайта оверлея для корректного отображения

        position = new Vector3(position.x, position.y, position.z + zOffset);
        return position;
    }
}
