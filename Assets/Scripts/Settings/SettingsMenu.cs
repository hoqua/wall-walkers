
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    private bool isOpen = false;

    private PlayerMovement _playerMovement;
    private GameObject playerObject;
    private void Start()
    {
        settingsMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
        settingsMenu.SetActive(isOpen);
        
        playerObject = GameObject.FindWithTag("Player");
        _playerMovement = playerObject.GetComponent<PlayerMovement>();
        
        if (playerObject != null && isOpen)
        {
            _playerMovement.enabled = false;
        }
        else if (_playerMovement != null && !isOpen)
        {
            _playerMovement.enabled = true;
        }
    }
}
