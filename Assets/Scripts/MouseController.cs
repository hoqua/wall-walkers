using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public GameObject characterPrefab;
    public GameObject spawnedCharacter;
    
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
                
                //Спавн персонажа
                if (spawnedCharacter == null)
                {
                    SpawnCharacterAtTile(overlayTile.transform.position);
                }
                else
                {
                    MoveCharacterToTile(overlayTile.transform.position);
                }
                
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

    private void SpawnCharacterAtTile(Vector3 spawnPosition)
    {
        spawnedCharacter = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
    }

    private void MoveCharacterToTile(Vector3 newPosition)
    {
        spawnedCharacter.transform.position = newPosition;
    }
    
}