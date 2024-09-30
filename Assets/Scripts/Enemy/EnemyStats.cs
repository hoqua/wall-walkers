
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private PlayerStats _player;
    
    public int health = 2;       // Количество здоровья врага
    public int damage = 1;       // Сколько урона наносит враг
    public int attackRange = 1;  // Радиус атаки (1 = одна клетка)

    void Start()
    {
        _player = FindObjectOfType<PlayerStats>();
    }
    
    public void TakeDamage(int playerDamage)
    {
        health -= playerDamage;
        Debug.Log($"Enemy takes {playerDamage} damage. Current health = {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died");
        _player.GainExp();
        Destroy(gameObject);
    }
}
