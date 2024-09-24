using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    public Animator animator;           // Аниматор и значения для анимаций
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
    
    private Transform _playerTransform;  // Положение игрока
    public int damage = 1;               // Урон игрока
    public EnemyStats targetEnemy;       // Враг которого игрок будет атаковать
    private static readonly int ReturnToIdleTrigger = Animator.StringToHash("ReturnToIdleTrigger");


    void Start()
    {
        animator = GetComponent<Animator>();
        _playerTransform = transform;
    }
    
    public void Attack(GameObject enemy)
    {
        // Направление атаки
        var direction = (enemy.transform.position - _playerTransform.position).normalized;

        // Задаем направление атаки в аниматоре
        SetAttackDirectionInAnimator(direction);
        
        animator.SetTrigger(AttackTrigger);
        
        // Нанесение урона
        var enemyStats = enemy.GetComponent<EnemyStats>();
        targetEnemy = enemyStats;
        targetEnemy.TakeDamage(damage);
        
        // Переход в Idle после атаки
        Invoke(nameof(ReturnToIdle), 0.7f);
    }

    private void SetAttackDirectionInAnimator(Vector3 direction)
    {
        float horizontal = Mathf.Round(direction.x);
        float vertical = Mathf.Round(direction.y);
        
        Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
        
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
        if (enemyStats != null) return enemyStats.health - damage <= 0;
        return false;
    }
}