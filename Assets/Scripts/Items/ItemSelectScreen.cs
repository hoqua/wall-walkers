using System.Collections;
using UnityEngine;

namespace Items
{
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

            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                _playerMovement = playerObject.GetComponent<PlayerMovement>();
                _playerMovement.enabled = false;
            }
            else
            {
                Debug.LogWarning("Player not found");
            }
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
            if (_playerMovement != null)
            {
                _playerMovement.enabled = true;
            }
            else
            {
                Debug.LogWarning("Player not found. Can't enable the movement after item screen exits.");
            }
        
        }
    
        // Автоматически скрывает окно при выходе из игры
        private void OnApplicationQuit()
        {
            itemSelectCanvas.SetActive(false);
        }
    }
}
