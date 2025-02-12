using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mimic : MonoBehaviour
{
    public GameObject attackVFXPrefab;  // Префаб эффекта удара
    private const float AttackVFXDuration = 0.5f; // Длительность эффекта атаки

    private EnemyStats _enemyStats;
    private PlayerStats _player;
    private Vector3Int _spawnPosition;
    private bool _hasAttacked = false;

    private void Start()
    {
        _enemyStats = GetComponent<EnemyStats>();
        _player = FindObjectOfType<PlayerStats>();
        attackVFXPrefab.SetActive(false);
        transform.localPosition += new Vector3(0, 0.25f, 0);
    }

    private void Update()
    {
        if (_player != null & IsPlayerInRange())
        {
            AttackPlayer();
        }
    }
    
    private bool IsPlayerInRange()
    {
        {
            Vector3Int enemyTile = Vector3Int.FloorToInt(transform.position);
            Vector3Int playerTile = Vector3Int.FloorToInt(_player.transform.position);

            int dx = Mathf.Abs(enemyTile.x - playerTile.x);
            int dy = Mathf.Abs(enemyTile.y - playerTile.y);
            return dx <= _enemyStats.attackRange && dy <= _enemyStats.attackRange;
        }
    }
    
    private void AttackPlayer()
    {
        _player.TakeDamage(_enemyStats.damage);
        
        ShowAttackEffect();
    }
    
    private void ShowAttackEffect()
    {
        attackVFXPrefab.SetActive(true);

        // Перемещаем эффект к игроку
        Vector3 playerPosition = _player.transform.position + new Vector3(0, 0, 10);
        attackVFXPrefab.transform.position = playerPosition;

        // Выключаем эффект через время
        Invoke(nameof(DeactivateSlash), AttackVFXDuration);
    }
    
    private void DeactivateSlash()
    {
        attackVFXPrefab.SetActive(false);  
    }
}
