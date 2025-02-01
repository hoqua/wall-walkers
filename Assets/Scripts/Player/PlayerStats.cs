using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
   private int _level = 1;                   // Уровень игрока
   [SerializeField] public int health = 5;  // Здоровье игрока (начальное)
   [SerializeField] public int damage = 1;   // Урон игрока (начальный)

   private int _currentExp = 0;
   private int _requiredExp = 1;
   private int _allExp = 0;
   private TMP_Text _levelText;    // Ссылка для отображения уровня
   private TMP_Text _healthText;   // Ссылка для отображения здоровья
   private TMP_Text _damageText;   // Ссылка для отображения урона
   private Slider _expSlider;      // Ссылка для отображения полоски опыта
   private ItemSelectScreen _itemSelectScreen; // Ссылка на скрипт для отображения экрана с выбором предметов

   void Start()
   {
      _levelText = GameObject.FindWithTag("LevelText").GetComponent<TMP_Text>();
      _expSlider = GameObject.FindWithTag("ExpSlider").GetComponent<Slider>();
      _healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
      _damageText = GameObject.FindWithTag("DamageText").GetComponent<TMP_Text>();

      SetUpExpSlider();
      UpdateAllUI();
      
      _itemSelectScreen = GameObject.Find("Item Select Screen").GetComponent<ItemSelectScreen>();
   }

   

   public void TakeDamage(int enemyDamage)
   {
      health -= enemyDamage;
      UpdateHealthUI();
      Debug.Log($"Player took {enemyDamage} damage. Current health = {health}");
      
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
   
   private void LevelUp()
   {
      _level += 1;
      health = 5 + _level;
      damage = 1 + damage;

      _currentExp = 0;
      _requiredExp *= 2;
      
      _expSlider.maxValue = _requiredExp;
      _expSlider.value = _currentExp;
    
      UpdateAllUI();
      
      StartCoroutine(WaitForTurnsToEnd()); // Запускаем корутину ожидания конца PlayerTurn и EnemyTurn
   }

   private IEnumerator WaitForTurnsToEnd()
   {
      GameManager gameManager = FindObjectOfType<GameManager>();

      // 1. Ждём, пока не закончится PlayerTurn
      while (gameManager.CurrentState() == GameState.PlayerTurn)
      {
         yield return null; // Ждём следующий кадр
      }

      // 2. Ждём, пока не закончится EnemyTurn
      while (gameManager.CurrentState() == GameState.EnemyTurn)
      {
         yield return null; // Ждём следующий кадр
      }

      // 3. Только теперь показываем экран выбора предметов
      _itemSelectScreen.ShowItemSelectScreen();
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
