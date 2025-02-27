using UnityEngine;
using UnityEngine.Tilemaps;

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
        
        private void Start()
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
                
                
                Vector3Int mimicTile = Tilemap.WorldToCell(transform.position);
                Vector3Int playerTile = Tilemap.WorldToCell(_player.transform.position);
                
                // Получаем позиции мимика и игрока в клетках
                var dx = Mathf.Abs(mimicTile.x - playerTile.x);
                var dy = Mathf.Abs(mimicTile.y - playerTile.y);
    
                var distance = Mathf.Max(dx, dy); 
    
                if (distance <= 1) // Если игрок находится в радиусе 1 клетки
                {
                    AttackPlayer();
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
                GameObject effect = Instantiate(attackEffectPrefab, _player.transform.position, Quaternion.identity);
                effect.SetActive(true);
                Destroy(effect, attackEffectDuration);
            }
            else
            {
                Debug.LogError("Ошибка: Префаб эффекта атаки не установлен в мимике!");
            }
        }
        
        private void OnDestroy()
        {
            Debug.LogWarning(gameObject.name + " был уничтожен!");
        }
    }
}