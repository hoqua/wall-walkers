using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        private ItemSelectScreen _itemSelectScreen;
        private PlayerStats _playerStats;

        private string effect;
        private Dictionary<string, Action> _effects;
        private void Awake()
        {
            _itemSelectScreen = GameObject.Find("Item Select Screen").GetComponent<ItemSelectScreen>();
        
            TextMeshProUGUI textComponent = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        
            if (textComponent != null)
            {
                effect = textComponent.text;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI not found");
            }
        
            _effects = new Dictionary<string, Action>()
            {
                {"Damage +1", () => _playerStats.damage += 1},
                {"Max Health +1", () => { _playerStats.health += 1; _playerStats.maxHealth += 1; }},
            };
        }

        private void OnMouseDown()
        {
            _playerStats = GameObject.Find("Player(Clone)").GetComponent<PlayerStats>();

            if (_effects.ContainsKey(effect))
            {
                _effects[effect].Invoke();
            }
            else
            {
                Debug.Log(effect + " doesn't exist");
            }
       
            _playerStats.UpdateAllUI();
            _itemSelectScreen.HideItemSelectScreen();
        }
    }
}
