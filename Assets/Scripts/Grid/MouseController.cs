using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float speed;
    public GameObject characterPrefab;
    private CharacterInfo _character;

    private PathFinder _pathFinder;
    private List<OverlayTile> _path;

    private void Start()
    {
        _pathFinder = new PathFinder();
        _path = new List<OverlayTile>();
    }

    private void LateUpdate()
    {
        var hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            var tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder =
                tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
            if (Input.GetMouseButtonDown(0))
            {
                tile.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                if (_character == null)
                {
                    _character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnLine(tile);
                    _character.standingOnTile = tile;
                }
                else
                {
                    _path = _pathFinder.FindPath(_character.standingOnTile, tile);

                    tile.gameObject.GetComponent<OverlayTile>().HideTile();
                }
            }
        }

        if (_path.Count > 0) MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        var zIndex = _path[0].transform.position.z;
        _character.transform.position =
            Vector2.MoveTowards(_character.transform.position, _path[0].transform.position, step);
        _character.transform.position =
            new Vector3(_character.transform.position.x, _character.transform.position.y, zIndex);

        if (Vector2.Distance(_character.transform.position, _path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(_path[0]);
            _path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        _character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f,
            tile.transform.position.z);
        _character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        _character.standingOnTile = tile;
    }

    private static RaycastHit2D? GetFocusedOnTile()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePos2D = new Vector2(mousePos.x, mousePos.y);

        var hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0) return hits.OrderByDescending(i => i.collider.transform.position.z).First();

        return null;
    }
}