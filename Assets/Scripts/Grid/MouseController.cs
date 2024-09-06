using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public TileCoordinates tileCoordinatesScript;
    
    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            GameObject overlayTile = hit.Value.collider.gameObject;
            cursor.transform.position = overlayTile.transform.position;
            cursor.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.GetComponent<OverlayTile>().ShowTile();

                MoveCharacterToTile(overlayTile.transform.position);
            }
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    void MoveCharacterToTile(Vector3 tilePosition)
    {
        if (tileCoordinatesScript.spawnedCharacter != null)
        {
            tileCoordinatesScript.spawnedCharacter.transform.position = tilePosition;
        }
    }
    
}