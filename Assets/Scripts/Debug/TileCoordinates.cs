using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCoordinates : MonoBehaviour
{
    public Tilemap tilemap; 
    public GameObject textPrefab;
    public GameObject parentObject; // Пустой объект для хранения всех координат

    public GameObject characterPrefab;
    public GameObject spawnedCharacter;
    private List<Vector3Int> validTilePositions = new List<Vector3Int>(); // Храним все допустимые позиции тайлов
    void Start()
    {
        DisplayTileCoordinates();
        
        SpawnCharacterAtRandomTile();
    }
    
    void SpawnCharacterAtRandomTile()
    {
        if (validTilePositions.Count == 0)
        {
            Debug.LogError("Нет доступных тайлов для спавна персонажа.");
            return;
        }
        
        Vector3Int randomTilePosition = validTilePositions[Random.Range(0, validTilePositions.Count)];
        Vector3 worldPosition = tilemap.GetCellCenterWorld(randomTilePosition);
        
        spawnedCharacter = Instantiate(characterPrefab, worldPosition, Quaternion.identity);
    }
    
    void DisplayTileCoordinates()
    {
        BoundsInt bounds = tilemap.cellBounds;
        
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                for (int z = bounds.zMin; z < bounds.zMax; z++) 
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, z);

                    if (tilemap.HasTile(tilePosition)) 
                    {
                        Vector3 worldPosition = tilemap.GetCellCenterWorld(tilePosition); 

                        // Смещение текста 
                        Vector3 textPosition = worldPosition + new Vector3(0, 0.05f, 10);
                        
                        GameObject textObject = Instantiate(textPrefab, textPosition, Quaternion.identity);
                        
                        // Текст становится дочерним элементом Tile Coordinates
                        textObject.transform.SetParent(parentObject.transform, true);
                        
                        TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
                        textMesh.text = $"({x}, {y}, {z})"; 
                        
                        // Добавляем текущую позицию тайла в список допустимых позиций
                        validTilePositions.Add(tilePosition);
                    }
                }
            }
        }
    }
    
}

