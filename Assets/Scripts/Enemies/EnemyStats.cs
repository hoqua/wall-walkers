using Damage_Text;
using UnityEngine;

namespace Enemies
{
    public class EnemyStats : MonoBehaviour
    {
        private PlayerStats _playerStats;
    
        public int health = 2;       // Количество здоровья врага
        public int damage = 1;       // Сколько урона наносит враг
        public int attackRange = 1;  // Радиус атаки (1 = одна клетка)

        void Start()
        {
            _playerStats = FindObjectOfType<PlayerStats>();
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
}
