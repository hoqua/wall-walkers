using System;
using Damage_Text;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class EnemyStats : MonoBehaviour
{
    private PlayerStats _playerStats;
    public Tilemap tilemap;
    public Vector3Int CurrentTile { get; private set; } 
        
    public int health = 2;       // Количество здоровья врага
    public int damage = 1;       // Сколько урона наносит враг
    public int attackRange = 1;  // Радиус атаки (1 = одна клетка)

    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>(); 
        tilemap = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        EnemyPositionManager.Instance.RegisterEnemy(CurrentTile);
        UpdateCurrentTile();
    }
    
    private void OnDestroy()
    {
        EnemyPositionManager.Instance.UnregisterEnemy(CurrentTile);
    }

    public void UpdateCurrentTile()
    {
        CurrentTile = tilemap.WorldToCell(transform.position);
    }
    
    public void TakeDamage(int playerDamage)
    {
        health -= playerDamage;
        FindObjectOfType<DamageTextSpawner>().SpawnDamageText(transform.position, _playerStats.damage);

        if (health <= 0) 
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died");
        _playerStats.GainExp();
        Destroy(gameObject);
    }
}

