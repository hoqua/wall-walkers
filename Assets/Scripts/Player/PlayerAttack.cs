using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;                      // Аниматор и значения для анимаций
    public GameObject slashObject;                 // Префаб для эффекта удара
    private readonly float _attackDuration = 0.7f; // Длительность анимации удара
    private readonly float _slashDuration = 0.5f;  // Длительность анимации эффекта удара (slash)
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
    private static readonly int ReturnToIdleTrigger = Animator.StringToHash("ReturnToIdleTrigger"); 
    
    private Transform _playerTransform;             // Положение игрока
    private PlayerStats _playerStats;               // Статистики игрока (здоровье, урон и тд.)
    private PlayerMovement _playerMovementScript;   // Ссылка на движение игрока
    public bool hasAttacked = false;                // Проверка, совершил ли игрок атаку
    
    void Start()
    {
        animator = GetComponent<Animator>();
        _playerTransform = transform;
        _playerMovementScript = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<PlayerStats>();
        _playerMovementScript = GetComponent<PlayerMovement>();
        slashObject.SetActive(false);
    }
    
    // Основная функция для атаки врага
    public void HandleAttack(Vector3Int targetTile)
    {
        GameObject enemy = FindEnemyOnTile(targetTile);

        if (enemy != null)
        {
            bool willKillEnemy = CheckIfWillKillEnemy(enemy);

            Attack(enemy);

            if (willKillEnemy)
            {
                _playerMovementScript.MoveToTile(targetTile);
            }
        }
        else
        {
            hasAttacked = false; // Не атакуем, если враг не найден
        }
    }

    public void Attack(GameObject enemy)
    {
        if (!hasAttacked)
        {
            hasAttacked = true;
            
            var direction = (enemy.transform.position - _playerTransform.position).normalized;
            SetAttackDirectionInAnimator(direction);

            animator.SetTrigger(AttackTrigger);
            ShowSlashEffect(direction);

            var enemyStats = enemy.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(_playerStats.damage);

            Invoke(nameof(ReturnToIdle), _attackDuration);
        }
    }

    public void ResetAttack()
    {
        hasAttacked = false;
    }

    private GameObject FindEnemyOnTile(Vector3Int targetTile)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            var enemyMovement = enemy.GetComponent<EnemyMovement>();

            if (enemyMovement.currentTile == targetTile)
            {
                return enemy;
            }
        }

        return null;
    }

    private bool CheckIfWillKillEnemy(GameObject enemy)
    {
        var enemyStats = enemy.GetComponent<EnemyStats>();
        return enemyStats.health - _playerStats.damage <= 0;
    }

    private void SetAttackDirectionInAnimator(Vector3 direction)
    {
        float horizontal = Mathf.Round(direction.x);
        float vertical = Mathf.Round(direction.y);
        
        animator.SetFloat(Horizontal, horizontal);
        animator.SetFloat(Vertical, vertical);
    }
    
    private void ShowSlashEffect(Vector3 direction)
    {
        slashObject.SetActive(true);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        slashObject.transform.rotation = Quaternion.Euler(0, 0, angle - 40);
        
        Invoke(nameof(DeactivateSlash), _slashDuration);
    }

    private void DeactivateSlash()
    {
        slashObject.SetActive(false);
    }

    private void ReturnToIdle()
    {
        animator.SetTrigger(ReturnToIdleTrigger);
        animator.SetFloat(Horizontal, 0);
        animator.SetFloat(Vertical, -1);
    }
}
