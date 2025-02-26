using Damage_Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
   private GameManager _gameManager;
   private XPBar _xpBar;

   private int _level = 1;                      // Уровень игрока
   [SerializeField] public int health = 5;      // Здоровье игрока (начальное)
   [SerializeField] public int maxHealth;       // Максимальное здоровье игрока
   [SerializeField] public int damage = 1;      // Урон игрока (начальный)
   

   public int currentExp = 0;
   public int requiredExp = 1;
   private int _allExp;
   
   private TMP_Text _levelText;    // Ссылка для отображения уровня
   private TMP_Text _healthText;   // Ссылка для отображения здоровья
   private TMP_Text _damageText;   // Ссылка для отображения урона
   private Slider _expSlider;      // Ссылка для отображения полоски опыта
   

   void Start()
   {
      _gameManager = FindObjectOfType<GameManager>();
      _xpBar = FindObjectOfType<XPBar>().GetComponent<XPBar>();
      _levelText = GameObject.FindWithTag("LevelText").GetComponent<TMP_Text>();
      _healthText = GameObject.FindWithTag("HealthText").GetComponent<TMP_Text>();
      _damageText = GameObject.FindWithTag("DamageText").GetComponent<TMP_Text>();
      
      SetUpExpSlider();
      
      maxHealth = health;
      UpdateAllUI();
      
      
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
      currentExp += 1;
      
      _xpBar.UpdateXPBar();
      
      if (currentExp >= requiredExp)
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

      currentExp = 0;
      requiredExp *= 2;
      
    
      UpdateAllUI();
      _gameManager.SetItemSelectionState(true);
   }
   
   public void UpdateAllUI()
   {
      UpdateDamageUI();
      UpdateHealthUI();
      UpdateLevelUI();
      _xpBar.UpdateXPBar();
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
         _expSlider.maxValue = requiredExp;
         _expSlider.value = currentExp;
      }
   }
   
   
   private void Die()
   {
      Debug.Log("Player has died");
      Destroy(gameObject);
   }
   
   
}
