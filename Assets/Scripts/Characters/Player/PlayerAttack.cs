
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
   public int damage = 1;           // Урон игрока
   public EnemyStats targetEnemy;   // Враг которого игрок будет атаковать
   
   public void Attack(GameObject enemy)
   {
       var enemyStats = enemy.GetComponent<EnemyStats>();
       targetEnemy = enemyStats;
       targetEnemy.TakeDamage(damage);
   }

   public bool CheckIfWillKillEnemy(GameObject enemy)
   {
       var enemyStats = enemy.GetComponent<EnemyStats>();
       if (enemyStats != null)
       {
           return enemyStats.health - damage <= 0;
       }
       return false;
   }
}
