using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    private GameObject _itemSelectionCanvas;
    private PlayerMovement _playerMovement;
    private void Start()
    {
        _itemSelectionCanvas = GameObject.Find("Item Selection Canvas");
        _itemSelectionCanvas.SetActive(false);
    }

    public void ShowItemSelectScreen()
    {
        _itemSelectionCanvas.SetActive(true);
        
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
    }

    public void HideItemSelectScreen()
    {
        _itemSelectionCanvas.SetActive(false); 
        _playerMovement.enabled = true;
    }
}
