using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        private ItemSelectScreenSoundController _itemSelectScreenSoundController;
        private ItemSelectMenu _itemSelectMenu;
        private PlayerStats _playerStats;

        private string _effect;
        private Dictionary<string, Action> _effects;
        private void Awake()
        {
            _itemSelectScreenSoundController = GameObject.Find("Item Select Menu").GetComponent<ItemSelectScreenSoundController>();
            
            _itemSelectMenu = GameObject.Find("Item Select Menu").GetComponent<ItemSelectMenu>();
            TextMeshProUGUI textComponent = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        
            if (textComponent != null)
            {
                _effect = textComponent.text;
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
            if (_itemSelectMenu.isMouseInputBlocked) return;

            _playerStats = GameObject.Find("Player(Clone)").GetComponent<PlayerStats>();

            if (_effects.ContainsKey(_effect))
            {
                _effects[_effect].Invoke();
                _itemSelectScreenSoundController.PlayMenuSelectSound();
            }
            else
            {
                Debug.Log(_effect + " doesn't exist");
            }
       
            _playerStats.UpdateAllUI();
            _itemSelectMenu.HideItemSelectScreen();
        }
    }
}
