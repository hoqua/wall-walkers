using UnityEngine;
using UnityEngine.UI;

public class DevMenu : MonoBehaviour
{
    public Toggle devMenuToggle;
    public GameObject devMenu;

    public Toggle gridGizmosToggle;
    public GameObject gridGizmos;

    public Toggle tileCoordinatesToggle;
    public GameObject tileCoordinates;
    public void Start()
    {
        devMenuToggle.isOn = false;
        devMenu.SetActive(false);

        gridGizmosToggle.isOn = false;
        gridGizmos.SetActive(false);

        tileCoordinatesToggle.isOn = false;
        tileCoordinates.SetActive(false);
    }

    public void ShowDevMenu()
    {
        devMenu.SetActive(!devMenu.activeSelf);
        
    }

    public void ShowGridGizmos()
    {
        gridGizmos.SetActive(!gridGizmos.activeSelf);
    }

    public void ShowTileCoordinates()
    {
        tileCoordinates.SetActive(!tileCoordinates.activeSelf);
    }
}
