using UnityEngine;

namespace Enemies.Mimic
{
    public class Mimic : Enemy
    {
        private GameManager _gameManager;
        private EnemyStats _enemyStats;
        private PlayerMovement _player;
        private PlayerStats _playerStats;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.AddEnemy(this);
            Debug.Log("Мимик создан!"); // Проверяем, создается ли мимик
        }

        public override void EnemyTurn()
        {
            if (this != null)
            { 
                if (_player == null) _player = FindObjectOfType<PlayerMovement>(); 
                if (_playerStats == null) _playerStats = FindObjectOfType<PlayerStats>();
                if (_enemyStats == null) _enemyStats = GetComponent<EnemyStats>();


                float distance = Vector3.Distance(transform.position, _player.transform.position);
                Debug.Log("Расстояние до игрока: " + distance);

                if (distance <= _enemyStats.attackRange)
                {
                    AttackPlayer();
                }
            }
        }

        private void AttackPlayer()
        {
            Debug.Log("Мимик атакует игрока!");
            _playerStats.TakeDamage(_enemyStats.damage);
        }
    }
}