using UnityEngine;

public class Chest : MonoBehaviour
{
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _mimicPrefab;
    private void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
    }
    public void OpenChest()
    {
        // Смещаем сундук перед уничтожением вниз, чтобы правильно появился предмет
        var adjustedPosition = transform.position;
        adjustedPosition.y -= 0.25f;
        transform.position = adjustedPosition;
        
        SpawnLoot();
        Destroy(gameObject);
    }

    private void SpawnLoot()
    {
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is not assigned to the chest!");
            return;
        }
        
        Vector3Int spawnPosition = _spawnManager.tilemap.WorldToCell(transform.position); 
        
        float randomValue = Random.value;

        if (randomValue < 0.1f)
        { 
            Instantiate(_mimicPrefab, _spawnManager.tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);
            Debug.Log("A mimic has spawned instead of a chest!");
        } 
        else if (randomValue < 0.5f) 
        {
            _spawnManager.SpawnItem(_spawnManager.healthPotionPrefab, _spawnManager.healthPotionContainer, spawnPosition, "HealthPotion");
        }
        else
        {
            _spawnManager.SpawnItem(_spawnManager.expGemPrefab, _spawnManager.expGemsContainer, spawnPosition, "ExpGem");
        }
        
    }

}