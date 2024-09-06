using UnityEngine;
using UnityEngine.Serialization;

public class OverlayTile : MonoBehaviour
{
    [FormerlySerializedAs("G")] public int g;
    [FormerlySerializedAs("H")] public int h;
    public int F => g + h;

    public bool isBlocked = false;

    [FormerlySerializedAs("Previous")] public OverlayTile previous;

    public Vector3Int gridLocation;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

}