using Enemies;
using UnityEngine;

public class Mimic : Enemy
{
    private EnemyStats _enemyStats;
    private PlayerMovement _player;
    private PlayerStats _playerStats;

    private void Start()
    {
        _enemyStats = GetComponent<EnemyStats>(); 
        _player = FindObjectOfType<PlayerMovement>();
        _playerStats = FindObjectOfType<PlayerStats>(); 

        Debug.Log("Мимик создан!"); // Проверяем, создается ли мимик
    }
    
    public override void EnemyTurn()
    {
        Debug.Log("Мимик выполняет ход!");

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        Debug.Log("Расстояние до игрока: " + distance);

        if (distance <= _enemyStats.attackRange)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("Мимик атакует игрока!");
        _playerStats.TakeDamage(_enemyStats.damage);
    }
}