using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
   public Tilemap tilemap; // Tilemap для спавна персонажей
   public Camera mainCamera;
   
   public GameObject playerPrefab;
   public GameObject enemyPrefab; 
   
   private Vector3Int _playerSpawnPosition;
   private Vector3Int _enemySpawnPosition;

   void Start()
   {
      _playerSpawnPosition = GetRandomSpawnPosition();
      _enemySpawnPosition = GetRandomSpawnPosition();

      while (_enemySpawnPosition == _playerSpawnPosition)
      {
         _enemySpawnPosition = GetRandomSpawnPosition();
      }

      GameObject player = Instantiate(playerPrefab, tilemap.GetCellCenterWorld(_playerSpawnPosition), Quaternion.identity);
      player.GetComponent<PlayerMovement>().SetCurrentTile(_playerSpawnPosition, tilemap);

      GameObject enemy = Instantiate(enemyPrefab, tilemap.GetCellCenterWorld(_enemySpawnPosition), Quaternion.identity);
      enemy.GetComponent<EnemyMovement>().SetCurrentTile(_enemySpawnPosition, tilemap);
      
      Debug.Log($"Player spawned at: {_playerSpawnPosition}, Enemy spawned at: {_enemySpawnPosition}");
   }
   
   private Vector3Int GetRandomSpawnPosition()
   {
      Vector3Int randomPosition;
      BoundsInt visibleBounds = GetVisibleTileBounds();

      do
      {
         int randomX = Random.Range(visibleBounds.xMin, visibleBounds.xMax);
         int randomY = Random.Range(visibleBounds.yMin, visibleBounds.yMax);
         randomPosition = new Vector3Int(randomX, randomY, 0);

      } while (!TileExists(randomPosition));

      return randomPosition;
   }
   private BoundsInt GetVisibleTileBounds()
   {
      Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
      Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

      Vector3Int bottomLeftTile = tilemap.WorldToCell(bottomLeft);
      Vector3Int topRightTile = tilemap.WorldToCell(topRight);

      return new BoundsInt(bottomLeftTile, topRightTile - bottomLeftTile);
   }
   
   private bool TileExists(Vector3Int position)
   {
      TileBase tile = tilemap.GetTile(position);
      return tile != null;
   }

  
}
