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
    public EnemyStats targetEnemy;                  // Враг которого игрок будет атаковать
    public bool hasAttacked = false;                // Проверка, совершил ли игрок атаку
    void Start()
    {
        animator = GetComponent<Animator>();
        _playerTransform = transform;
        _playerMovementScript = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<PlayerStats>();
        slashObject.SetActive(false);
    }
    
    public void HandleAttack(GameObject enemy, Vector3Int targetTile)
    {
        bool willKillEnemy = CheckIfWillKillEnemy(enemy);
        
        Attack(enemy);

        // Если следующая атака убъет врага, перемещаемся на его клетку
        if (willKillEnemy)
        {
            _playerMovementScript.MoveToTile(targetTile);
        }
    }
    
    public void Attack(GameObject enemy)
    {
        if (!hasAttacked)
        {
            // Устанавливается флаг, что игрок атаковал
            hasAttacked = true;
            
            // Направление атаки
            var direction = (enemy.transform.position - _playerTransform.position).normalized;

            // Задаем направление атаки в аниматоре
            SetAttackDirectionInAnimator(direction);

            animator.SetTrigger(AttackTrigger);

            // Эффект удара
            ShowSlashEffect(direction);

            // Нанесение урона
            var enemyStats = enemy.GetComponent<EnemyStats>();
            targetEnemy = enemyStats;
            targetEnemy.TakeDamage(_playerStats.damage);

            // Переход в Idle после атаки
            Invoke(nameof(ReturnToIdle), _attackDuration);
        }
    }

    public void ResetAttack()
    {
        hasAttacked = false;
    }

    // Слеш эффект
    private void ShowSlashEffect(Vector3 direction)
    {
        slashObject.SetActive(true);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        slashObject.transform.rotation = Quaternion.Euler(0, 0, angle - 40);
        
        Invoke(nameof(DeactivateSlash), _slashDuration);
    }

    // Деактивация слеш эффекта
    private void DeactivateSlash()
    {
        slashObject.SetActive(false);
    }

    private void SetAttackDirectionInAnimator(Vector3 direction)
    {
        float horizontal = Mathf.Round(direction.x);
        float vertical = Mathf.Round(direction.y);
        
        animator.SetFloat(Horizontal, horizontal);
        animator.SetFloat(Vertical, vertical);
    }
    
    // Возвращает анимацию в исходное положение после атаки
    private void ReturnToIdle()
    {
        animator.SetTrigger(ReturnToIdleTrigger);
        animator.SetFloat(Horizontal, 0);
        animator.SetFloat(Vertical, -1);
    }

    public bool CheckIfWillKillEnemy(GameObject enemy)
    {
        var enemyStats = enemy.GetComponent<EnemyStats>();
        if (enemyStats != null) return enemyStats.health - _playerStats.damage <= 0;
        return false;
    }
}