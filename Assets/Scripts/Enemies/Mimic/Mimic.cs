using Enemies;
using UnityEngine;

public class Mimic : Enemy
{
    private EnemyStats _enemyStats;
    private PlayerMovement _player;
    private PlayerStats _playerStats;
    private bool _isAwakened = false;

    private void Start()
    {
        _enemyStats = GetComponent<EnemyStats>(); 
        _player = FindObjectOfType<PlayerMovement>();
        _playerStats = GetComponent<PlayerStats>();
        
    }

    public void AwakeMimic()
    {
        _isAwakened = true;
    }

    public override void EnemyTurn()
    {
        if (!_isAwakened) return; // Если мимик ещё не пробудился, он не действует

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= _enemyStats.attackRange)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        _playerStats.TakeDamage(_enemyStats.damage);
    }
}