using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public Animator animator;
    private EnemyAI enemyAI;
    private Health health;

    // نام پارامترهای انیمیشن
    private readonly string IS_MOVING = "IsMoving";
    private readonly string IS_ATTACKING = "IsAttacking";
    private readonly string IS_DEAD = "IsDead";
    private readonly string HIT = "Hit";
    private readonly string ATTACK = "Attack";

    void Start()
    {
        // دریافت کامپوننت‌های مورد نیاز
        enemyAI = GetComponent<EnemyAI>();
        health = GetComponent<Health>();

        // ثبت کردن به رویدادهای Health
        if (health != null)
        {
            health.OnDamageTaken += OnDamageTaken;
            health.OnDeath += OnDeath;
        }
    }

    void Update()
    {
        if (enemyAI != null)
        {
            // بررسی حرکت دشمن
            bool isMoving = enemyAI.IsMoving();
            animator.SetBool(IS_MOVING, isMoving);

            // بررسی حمله دشمن
            bool isAttacking = enemyAI.IsAttacking();
            animator.SetBool(IS_ATTACKING, isAttacking);

            // اگر در حال حمله است، تریگر انیمیشن حمله را فعال کن
            if (isAttacking)
            {
                animator.SetTrigger(ATTACK);
            }
        }
    }

    void OnDamageTaken()
    {
        // پخش انیمیشن ضربه
        animator.SetTrigger(HIT);
    }

    void OnDeath()
    {
        // پخش انیمیشن مرگ
        animator.SetBool(IS_DEAD, true);
    }

    void OnDestroy()
    {
        // حذف ثبت رویدادها
        if (health != null)
        {
            health.OnDamageTaken -= OnDamageTaken;
            health.OnDeath -= OnDeath;
        }
    }
} 