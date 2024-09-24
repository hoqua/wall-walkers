using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
   public int damage = 1;      // Сколько урона наносит враг
   public int attackRange = 1; // Радиус атаки (1 = одна клетка)
   
   private PlayerStats _player;

   void Start()
   {
      _player = FindObjectOfType<PlayerStats>();
   }

   // Проверяет есть ли в радиусе врага игрок
   public bool IsPlayerInRange(Vector3Int enemyTile, Vector3Int playerTile)
   {
      int dx = Mathf.Abs(enemyTile.x - playerTile.x);
      int dy = Mathf.Abs(enemyTile.y - playerTile.y);
      return dx <= attackRange && dy <= attackRange; // Находится ли игрок в радиусе атаки
   }
   
   public void AttackPlayer()
   {
      if (_player != null)
      {
         _player.TakeDamage(damage);
         Debug.Log($"Enemy attacks Player for {damage} damage.");
      }
   }

}
