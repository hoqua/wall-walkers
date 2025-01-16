using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemSelectScreen _itemSelectScreen;
    private PlayerStats _playerStats;
    private void Awake()
    {
        _itemSelectScreen = GameObject.Find("Item Select Screen").GetComponent<ItemSelectScreen>();
    }

    private void OnMouseDown()
    {
        _playerStats = GameObject.Find("Player(Clone)").GetComponent<PlayerStats>();
        _playerStats.damage += 1;
        _playerStats.UpdateAllUI();
        _itemSelectScreen.HideItemSelectScreen();
    }
}
