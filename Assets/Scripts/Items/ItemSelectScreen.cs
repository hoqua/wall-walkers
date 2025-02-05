using System.Collections;
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
        StartCoroutine(EnableMovementWithDelay(0.3f));
        
        gameManager.SetItemSelectionState(false);
    }

    private IEnumerator EnableMovementWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerMovement.enabled = true;
    }
}
