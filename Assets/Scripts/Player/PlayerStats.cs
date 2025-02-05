using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
   private GameManager _gameManager;
   private ItemSelectScreen _itemSelectScreen; // Ссылка на скрипт для отображения экрана с выбором предметов
   
   private int _level = 1;                      // Уровень игрока
   [SerializeField] public int health = 5;      // Здоровье игрока (начальное)
   [SerializeField] public int maxHealth;       // Максимальное здоровье игрока
   [SerializeField] public int damage = 1;      // Урон игрока (начальный)

   private int _currentExp = 0;
   private int _requiredExp = 1;
   private int _allExp = 0;
   
   private TMP_Text _levelText;    // Ссылка для отображения уровня
   private TMP_Text _healthText;   // Ссылка для отображения здоровья
   private TMP_Text _damageText;   // Ссылка для отображения урона
   private Slider _expSlider;      // Ссылка для отображения полоски опыта

   void Start()
   {
      _levelText = GameObject.FindWithTag("LevelText").GetComponent<TMP_Text>();
      _expSlider = GameObject.FindWithTag("ExpSlider").GetComponent<Slider>();
      _healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
      _damageText = GameObject.FindWithTag("DamageText").GetComponent<TMP_Text>();

      SetUpExpSlider();
      
      maxHealth = health;
      UpdateAllUI();
      
      _gameManager = FindObjectOfType<GameManager>();
      _itemSelectScreen = GameObject.Find("Item Select Screen").GetComponent<ItemSelectScreen>();
   }

   

   public void TakeDamage(int enemyDamage)
   {
      health -= enemyDamage;
      UpdateHealthUI();
      
      FindObjectOfType<DamageTextSpawner>().SpawnDamageText(transform.position, enemyDamage);
      
      if (health <= 0)
      {
         Die();
      }
   }

   public void GainExp()
   {
      _allExp += 1;
      _currentExp += 1;
      _expSlider.value = _currentExp;
      
      if (_currentExp >= _requiredExp)
      {
         LevelUp();
      }
   }

   public void HealToFull()
   {
      health = maxHealth;
      UpdateHealthUI();
   }
   
   private void LevelUp()
   {
      _level += 1;
      damage = 1 + damage;

      _currentExp = 0;
      _requiredExp *= 2;
      
      _expSlider.maxValue = _requiredExp;
      _expSlider.value = _currentExp;
    
      UpdateAllUI();
      _gameManager.SetItemSelectionState(true);
   }
   
   public void UpdateAllUI()
   {
      UpdateDamageUI();
      UpdateHealthUI();
      UpdateLevelUI();
   }
   
   private void UpdateLevelUI()
   {
      _levelText.text = $"{_level}";
   }
   
   void UpdateHealthUI()
   {
      _healthText.text = $"{health}";
   }

   void UpdateDamageUI()
   {
      _damageText.text = $"{damage}";
   }

   void SetUpExpSlider()
   {
      if (_expSlider != null)
      {
         _expSlider.maxValue = _requiredExp;
         _expSlider.value = _currentExp;
      }
   }
   
   private void Die()
   {
      Debug.Log("Player has died");
      Destroy(gameObject);
   }
   
   
}
