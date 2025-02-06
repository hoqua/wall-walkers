using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool _isOpened = false;

    public void OpenChest()
    {
        if (_isOpened) return;

        _isOpened = true;
        Debug.Log("Chest opened! Player received a reward.");
        
        Destroy(gameObject);
    }
}