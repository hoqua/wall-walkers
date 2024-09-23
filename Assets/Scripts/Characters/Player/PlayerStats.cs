using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   public int health = 4;

   public void TakeDamage(int damage)
   {
      health -= damage;
      Debug.Log($"Player took {damage} damage. Current health = {health}");

      if (health <= 0)
      {
         Die();
      }
   }

   private void Die()
   {
      Debug.Log("Player has died");
   }
}
