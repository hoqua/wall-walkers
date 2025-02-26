using UnityEngine;

namespace Enemies.Skeleton
{
   public class SkeletonAttack : MonoBehaviour
   {
      public GameObject slashObject;                // Префаб для эффекта удара
      private readonly float _slashDuration = 0.5f; // Длительность эффекта атаки (slash)

      private EnemyStats _enemyStats;
      private PlayerStats _player;
      private Transform _enemy;
   
      void Start()
      {
         _enemyStats = GetComponent<EnemyStats>();
         _enemy = GetComponent<Transform>();
         _player = FindObjectOfType<PlayerStats>();
         slashObject.SetActive(false);
      }
   
      // Проверяет есть ли в радиусе врага игрок
      public bool IsPlayerInRange(Vector3Int enemyTile, Vector3Int playerTile)
      {
         int dx = Mathf.Abs(enemyTile.x - playerTile.x);
         int dy = Mathf.Abs(enemyTile.y - playerTile.y);
         return dx <= _enemyStats.attackRange && dy <= _enemyStats.attackRange; // Находится ли игрок в радиусе атаки
      }
   
      public void AttackPlayer()
      {
         if (_player != null && _enemy != null)
         {
            _player.TakeDamage(_enemyStats.damage);
            Debug.Log($"Enemy attacks Player for {_enemyStats.damage} damage.");

            Vector3 attackDirection = (_player.transform.position - _enemy.position).normalized;
         
            ShowSlashEffect(attackDirection);
         }
      }

      // Слеш эффект
      private void ShowSlashEffect(Vector3 attackDirection)
      {
         slashObject.SetActive(true);

         float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
         slashObject.transform.rotation = Quaternion.Euler(0, 0, angle - 40);
      
         Invoke(nameof(DeactivateSlash), _slashDuration);
      }

      // Деактивация слеш эффекта
      private void DeactivateSlash()
      {
         slashObject.SetActive(false);  
      }
   }
}
