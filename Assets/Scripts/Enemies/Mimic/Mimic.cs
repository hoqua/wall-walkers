using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Enemies.Mimic
{
    public class Mimic : Enemy
    {
        private GameManager _gameManager;
        private EnemyStats _enemyStats;
        private PlayerMovement _player;
        private PlayerStats _playerStats;
        
        [SerializeField] private GameObject attackEffectPrefab; // 2 Префаба эффекта атаки для реализации "укуса"
        [SerializeField] private GameObject attackEffectPrefab2;
        private readonly float _attackEffectDuration = 0.3f; // Время жизни эффекта
        
        private MimicSoundController _mimicSoundController;
        
        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.AddEnemy(this);
            _mimicSoundController = GetComponent<MimicSoundController>();
        }

        public override void EnemyTurn()
        {
            if (this != null)
            {
                if (_player == null) _player = FindObjectOfType<PlayerMovement>();
                if (_playerStats == null) _playerStats = FindObjectOfType<PlayerStats>();
                if (_enemyStats == null) _enemyStats = GetComponent<EnemyStats>();

                if (_player == null && _playerStats == null)
                {
                    return;
                }
                
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
            _mimicSoundController.PlayMimicBiteSound();
        }
        
        private void ShowAttackEffect()
        {
            if (attackEffectPrefab != null)
            {
                var effectLocation = _player.transform.position + new Vector3(0, 0.2f, 0);
                
                GameObject effect = Instantiate(attackEffectPrefab, effectLocation, Quaternion.Euler(0, 0, -30)); // Задаем нужный угол для эффекта
                GameObject effect2 = Instantiate(attackEffectPrefab2, effectLocation, Quaternion.Euler(0, 0, -210));
                
                effect.SetActive(true);
                effect2.SetActive(true);
                
                Destroy(effect, _attackEffectDuration);
                Destroy(effect2, _attackEffectDuration);
            }
            else
            {
                Debug.LogError("Ошибка: Префаб эффекта атаки не установлен в мимике!");
            }
        }
    }
}