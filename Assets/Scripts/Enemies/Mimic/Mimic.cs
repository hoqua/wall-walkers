using UnityEngine;

namespace Enemies.Mimic
{
    public class Mimic : Enemy
    {
        private GameManager _gameManager;
        private EnemyStats _enemyStats;
        private PlayerMovement _player;
        private PlayerStats _playerStats;
        
        [SerializeField] private GameObject attackEffectPrefab; // Префаб эффекта атаки
        [SerializeField] private float attackEffectDuration = 1f; // Время жизни эффекта

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.AddEnemy(this);
        }

        public override void EnemyTurn()
        {
            if (this != null)
            { 
                if (_player == null) _player = FindObjectOfType<PlayerMovement>(); 
                if (_playerStats == null) _playerStats = FindObjectOfType<PlayerStats>();
                if (_enemyStats == null) _enemyStats = GetComponent<EnemyStats>();

                if (_player != null)
                {
                    float distance = Vector3.Distance(transform.position, _player.transform.position);

                    if (distance <= _enemyStats.attackRange)
                    {
                        AttackPlayer();
                    }
                }
            }
        }

        private void AttackPlayer()
        {
            _playerStats.TakeDamage(_enemyStats.damage);
            ShowAttackEffect();
        }
        
        private void ShowAttackEffect()
        {
            if (attackEffectPrefab != null)
            {
                var playerPosition = _player.transform.position;
                playerPosition.y = transform.position.y + 0.015f;   //Корректировка высоты отображения эффекта
                GameObject effect = Instantiate(attackEffectPrefab, playerPosition, Quaternion.identity);
                effect.SetActive(true);
                Destroy(effect, attackEffectDuration);
            }
            else
            {
                Debug.LogError("Ошибка: Префаб эффекта атаки не установлен в мимике!");
            }
        }
    }
}