using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   private int _level = 1;                   // Уровень игрока
   [SerializeField] private int health = 5;  // Здоровье игрока (начальное)
   [SerializeField] public int damage = 1;   // Урон игрока (начальный)

   private int _exp = 0;
   private int _requiredExp = 1;
   private TMP_Text _levelText;    // Ссылка на TMP для отображения уровня
   private TMP_Text _healthText;   // Ссылка на TMP для отображения здоровья
   private TMP_Text _damageText;   // Ссылка на TMP для отображения урона
   private ItemSelectScreen _itemSelectScreen; // Ссылка на скрипт для отображения экрана с выбором предметов

   void Start()
   {
      _levelText = GameObject.FindWithTag("LevelText").GetComponent<TMP_Text>();
      _healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
      _damageText = GameObject.FindWithTag("DamageText").GetComponent<TMP_Text>();

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
      _exp += 1;
      
      if (_exp >= _requiredExp)
      {
         LevelUp();
         _requiredExp *= 2;
      }
   }
   
   private void LevelUp()
   {
      _level += 1;
      health = 5 + _level;
      damage = 1 + damage;
    
      UpdateAllUI();

      // Запускаем корутину ожидания конца PlayerTurn и EnemyTurn
      StartCoroutine(WaitForTurnsToEnd());
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

   
   private void Die()
   {
      Debug.Log("Player has died");
      Destroy(gameObject);
   }
   
   
}
