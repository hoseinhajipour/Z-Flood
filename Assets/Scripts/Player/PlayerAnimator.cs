using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    private PlayerController_ playerController;
    private AutoShooter autoShooter;
    private Health health;

    // نام پارامترهای انیمیشن
    private readonly string IS_RUNNING = "IsRunning";
    private readonly string IS_SHOOTING = "IsShooting";
    private readonly string IS_DEAD = "IsDead";
    private readonly string HIT = "Hit";

    void Start()
    {
        // دریافت کامپوننت‌های مورد نیاز
     //   animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController_>();
        autoShooter = GetComponent<AutoShooter>();
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
        // بررسی حرکت بازیکن
        if (playerController != null)
        {
            bool isMoving = playerController.IsMoving();
            animator.SetBool(IS_RUNNING, isMoving);
        }

        // بررسی تیراندازی
        if (autoShooter != null)
        {
            bool isShooting = autoShooter.IsShooting();
            animator.SetBool(IS_SHOOTING, isShooting);
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