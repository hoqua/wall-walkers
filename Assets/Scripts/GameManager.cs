using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public PlayerMovement player; //Ссылка на скрипт игрока
   public EnemyMovement enemy; // Ссылка на скрипт врага

   public float checkInterval = 0.05f; // Интервал между попытками поиска
   private bool _playerFound;
   private bool _enemyFound;
   public Vector3Int targetTile;

   void Start()
   {
      StartCoroutine(FindPlayerAndEnemy());
   }

   private IEnumerator FindPlayerAndEnemy()
   {
      while (!_playerFound || !_enemyFound) 
      {
         if (!_playerFound)
         {
            PlayerMovement foundPlayer = FindObjectOfType<PlayerMovement>();

            if (foundPlayer != null)
            {
               player = foundPlayer;
               _playerFound = true;
               Debug.Log("Player found.");
            }
            else
            {
               Debug.Log("Player not found, trying again...");
            }
         }
         
         if (!_enemyFound)
         {
            EnemyMovement foundEnemy = FindObjectOfType<EnemyMovement>();

            if (foundEnemy != null)
            {
               enemy = foundEnemy;
               _enemyFound = true;
               Debug.Log("Enemy found.");
            }
            else
            {
               Debug.Log("Enemy not found, trying again...");
            }
         }
         
         yield return new WaitForSeconds(checkInterval);
      }

      Debug.Log("Both Player and Enemy found, game can proceed.");
   }
   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         mousePos.z = 0f;

         targetTile = player.tilemap.WorldToCell(mousePos);

         // Если игрок кликнет на себя, то ничего не произойдет
         if (targetTile == player.currentTile)
         {
            Debug.Log("Player is already on this tile.");
            return;
         }
         
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
