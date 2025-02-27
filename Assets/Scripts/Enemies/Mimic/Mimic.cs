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

                Debug.Log("Мимик выполняет ход!");

                if (_enemyStats.tilemap == null)
                {
                    _enemyStats.tilemap = FindObjectOfType<Tilemap>();
                }
                
                Vector3Int mimicTile = _enemyStats.tilemap.WorldToCell(transform.position);
                Vector3Int playerTile = _enemyStats.tilemap.WorldToCell(_player.transform.position);
                
                // Получаем позиции мимика и игрока в клетках
                int dx = Mathf.Abs(mimicTile.x - playerTile.x);
                int dy = Mathf.Abs(mimicTile.y - playerTile.y);
    
                int distance = Mathf.Max(dx, dy); 
                Debug.Log("Расстояние в клетках: " + distance);
    
                if (distance <= 1) // Если игрок находится в радиусе 1 клетки
                {
                    Debug.Log("Мимик атакует! Дистанция: " + distance);
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