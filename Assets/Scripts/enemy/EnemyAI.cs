using System.Collections;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    protected Transform player;

    public int health = 3;
    public float attackRange = 2f;  // فاصله ای که در آن دشمن شروع به حمله می‌کند
    public float attackCooldown = 2f;

    public int attackDamage = 10;
    private float lastAttackTime;
    private bool isMoving = false;
    private bool isAttacking = false;
    private Vector3 lastPosition;

    public Animator animator; // رفرنس به انیماتور دشمن

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        lastPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // بررسی حرکت
        isMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f;
        lastPosition = transform.position;

        if (distanceToPlayer <= attackRange)
        {
            // اگر در محدوده حمله است و زمان کولداون گذشته
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
        else
        {
            MoveTowardPlayer();
        }
    }

    protected virtual void MoveTowardPlayer()
    {
        if (player == null) return;

        // محاسبه جهت حرکت به سمت بازیکن
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0; // فقط حرکت در صفحه XZ

        // حرکت دشمن به سمت بازیکن
        transform.position += dir * moveSpeed * Time.deltaTime;

        // محاسبه چرخش به سمت بازیکن
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    protected virtual void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");  // اجرای انیمیشن حمله

        // آسیب به بازیکن
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        // مدت زمان حمله را در اینجا تنظیم کنید
        // می‌توانید از یک Coroutine برای صبر کردن بعد از انجام حمله استفاده کنید
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        // زمان لازم برای پایان انیمیشن حمله (باید مطابق با انیمیشن تنظیم شود)
        yield return new WaitForSeconds(0.5f);

        isAttacking = false; // بعد از پایان حمله، دشمن آماده حمله بعدی می‌شود
    }

    public virtual void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
            // امتیاز دادن و انفجار بعدا اضافه می‌شود
        }
    }

    // متد برای بررسی وضعیت حرکت
    public bool IsMoving()
    {
        return isMoving;
    }

    // متد برای بررسی وضعیت حمله
    public bool IsAttacking()
    {
        return isAttacking;
    }
}
