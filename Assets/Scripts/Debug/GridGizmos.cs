using UnityEngine;

public class IsometricGridGizmos : MonoBehaviour
{
    public int gridSizeX = 10;   
    public int gridSizeY = 10;    
    public float cellSize = 1f;   
    public Color gridColor = Color.green;  

    private Vector3 GetIsometricPosition(int x, int y)
    {
        float posX = (x - y) * cellSize * 0.5f;
        float posY = (x + y) * cellSize * 0.25f;
        return new Vector3(posX, posY, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;
        
        for (int x = -gridSizeX; x <= gridSizeX; x++)
        {
            Vector3 startPos = GetIsometricPosition(x, -gridSizeY);
            Vector3 endPos = GetIsometricPosition(x, gridSizeY);
            Gizmos.DrawLine(startPos, endPos);
        }
        
        for (int y = -gridSizeY; y <= gridSizeY; y++)
        {
            Vector3 startPos = GetIsometricPosition(-gridSizeX, y);
            Vector3 endPos = GetIsometricPosition(gridSizeX, y);
            Gizmos.DrawLine(startPos, endPos);
        }
    }

    void OnDrawGizmosSelected()
    {
        OnDrawGizmos();  
    }
}