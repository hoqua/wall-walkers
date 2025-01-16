using UnityEngine;

public class ItemSelectScreen : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private PlayerMovement _playerMovement;
    
    [SerializeField] private GameObject itemSelectCanvas;
    [SerializeField] private GameObject itemSelectText;
    private void Start()
    {
        itemSelectCanvas.SetActive(false);
        itemSelectText.SetActive(false);
    }

    public void ShowItemSelectScreen()
    {
        itemSelectCanvas.SetActive(true);
        itemSelectText.SetActive(true);
        
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
    }

    public void HideItemSelectScreen()
    {
        itemSelectText.SetActive(false);
        itemSelectCanvas.SetActive(false); 
        _playerMovement.enabled = true;
    }
}
