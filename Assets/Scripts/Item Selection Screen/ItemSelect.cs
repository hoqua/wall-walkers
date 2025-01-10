using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    private GameObject _itemSelectionCanvas;
    private void Start()
    {
        _itemSelectionCanvas = GameObject.Find("Item Selection Canvas");
        _itemSelectionCanvas.SetActive(false);
    }

    public void ShowItemSelectScreen()
    {
        _itemSelectionCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideItemSelectScreen()
    {
        _itemSelectionCanvas.SetActive(false);
        Time.timeScale = 1;
    }
}
