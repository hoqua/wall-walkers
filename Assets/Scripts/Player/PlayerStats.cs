using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   private readonly int _level = 1; // Уровень игрока
   public int health = 5;  // Здоровье игрока (начальное)
   public int damage = 1;  // Урон игрока (начальный)

   private TMP_Text _levelText;  // Ссылка на TMP для отображения уровня
   private TMP_Text _healthText; // Ссылка на TMP для отображения здоровья
   private TMP_Text _damageText; // Ссылка на TMP для отображения урона

   void Start()
   {
      _levelText = GameObject.FindWithTag("LevelText").GetComponent<TMP_Text>();
      _healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
      _damageText = GameObject.FindWithTag("DamageText").GetComponent<TMP_Text>();

      UpdateLevelUI();
      UpdateHealthUI();
      UpdateDamageUI();
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
