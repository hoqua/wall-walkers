using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionManager : MonoBehaviour
{
    private static EnemyPositionManager _instance;
    private HashSet<Vector3Int> _enemyPositions = new();  // Используем HashSet для быстрого поиска

    public static EnemyPositionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("EnemyPositionManager");
                _instance = obj.AddComponent<EnemyPositionManager>();
            }
            return _instance;
        }
    }

    public void RegisterEnemy(Vector3Int position)
    {
        _enemyPositions.Add(position);
    }

    public void UnregisterEnemy(Vector3Int position)
    {
        _enemyPositions.Remove(position);
    }

    public bool IsTileOccupied(Vector3Int position)
    {
        return _enemyPositions.Contains(position);
    }
}