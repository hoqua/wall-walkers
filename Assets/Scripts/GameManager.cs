using UnityEngine;


public class GameManager : MonoBehaviour
{
   public CharacterMovement player; //Ссылка на скрипт игрока
   public EnemyMovement enemy; // Ссылка на скрипт врага

   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         mousePos.z = 0f;

         var targetTile = player.tilemap.WorldToCell(mousePos);

         if (player.IsWithinOneTileRadius(targetTile) && player.TileExists(targetTile))
         {
            player.MoveToTile(targetTile);

            enemy.MoveTowardsPlayer();
         }
         
         else
         {
            Debug.Log("Cannot move player to the target tile.");
         }
      }
   }
}
