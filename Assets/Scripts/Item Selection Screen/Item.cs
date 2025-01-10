using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemSelect _itemSelect;
    private PlayerStats _playerStats;
    private void Awake()
    {
        _itemSelect = GameObject.Find("Item Select Screen").GetComponent<ItemSelect>();
    }

    private void OnMouseDown()
    {
        _playerStats = GameObject.Find("Player(Clone)").GetComponent<PlayerStats>();
        _playerStats.damage += 1;
        _playerStats.UpdateAllUI();
        _itemSelect.HideItemSelectScreen();
    }
}
