using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class OverlayContainer : MonoBehaviour
    {
        private static OverlayContainer _instance;
        public static OverlayContainer Instance { get { return _instance; }}

        public GameObject overlayTilePrefab;
        public GameObject overlayContainer;
    
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    
        // Start is called before the first frame update
        void Start()
        {
            var tileMap = gameObject.GetComponentInChildren<Tilemap>();

            BoundsInt bounds = tileMap.cellBounds;

            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        var tileLocation = new Vector3Int(x, y, z);

                        if (tileMap.HasTile(tileLocation))
                        {
                            var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                            var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z+1);
                            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        }
                    }
                }
            }
        }
    
    }
}
