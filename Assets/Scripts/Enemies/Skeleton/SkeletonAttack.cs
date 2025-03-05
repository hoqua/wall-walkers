using System.Collections;
using UnityEngine;

namespace Enemies.Skeleton
{
   public class SkeletonAttack : MonoBehaviour
   {
      public GameObject slashObject;                // Префаб для эффекта удара
      private readonly float _slashDuration = 0.5f; // Длительность эффекта атаки (slash)

      private SkeletonSoundController _skeletonSoundController;
      
      private EnemyStats _enemyStats;
      private PlayerStats _player;
      private Transform _enemy;
   
      void Start()
      {
         _enemyStats = GetComponent<EnemyStats>();
         _enemy = GetComponent<Transform>();
         _player = FindObjectOfType<PlayerStats>();
         _skeletonSoundController = GetComponent<SkeletonSoundController>();
         
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
            StartCoroutine(_skeletonSoundController.PlayDoubleSwordHitSound());
            ShowSlashEffect();
         }
      }
      
      // Слеш эффект
      private void ShowSlashEffect()
      {
         slashObject.SetActive(true);
         
         var effectPosition = _player.transform.position + new Vector3(0, 0.15f, 0); //Корректирует расположение эффекта
         slashObject.transform.position = effectPosition;
         
         Invoke(nameof(DeactivateSlash), _slashDuration);
      }

      // Деактивация слеш эффекта
      private void DeactivateSlash()
      {
         slashObject.SetActive(false);  
      }
   }
}
