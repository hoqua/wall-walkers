using UnityEngine;

public class EnemyController : MonoBehaviour
{
   private EnemyMovement _enemyMovement;
   
   void Start()
   {
      _enemyMovement = GetComponent<EnemyMovement>();
   }
   public void MoveTowardsPlayer()
   {
      _enemyMovement.MoveTowardsPlayer();  
   }
}
