using UnityEngine;

public class ItemSelectScreen : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private PlayerMovement _playerMovement;
    
    [SerializeField] private GameObject itemSelectCanvas;
   
    private void Start()
    {
        itemSelectCanvas.SetActive(false);
    }

    public void ShowItemSelectScreen()
    {
        itemSelectCanvas.SetActive(true);
        
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
    }

    public void HideItemSelectScreen()
    {
        itemSelectCanvas.SetActive(false); 
        _playerMovement.enabled = true;
    }
}
