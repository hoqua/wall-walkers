using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterTilemapSpawner : MonoBehaviour
{
    public Tilemap tilemap;           // Tilemap для спавна
    public GameObject characterPrefab; // Префаб персонажа
    public Vector3Int spawnTile;      // Координаты клетки для спавна персонажа

    void Start()
    {
        SpawnCharacterAtTile(spawnTile);
    }
    
    void SpawnCharacterAtTile(Vector3Int tilePosition)
    {
        Vector3 spawnPosition = tilemap.GetCellCenterWorld(tilePosition);
        GameObject newCharacter = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

        
        CharacterMovement characterMovement = newCharacter.GetComponent<CharacterMovement>();
        if (characterMovement != null)
        {
            characterMovement.SetCurrentTile(tilePosition, tilemap); 
        }
    }
}