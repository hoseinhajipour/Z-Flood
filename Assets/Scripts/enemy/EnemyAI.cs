using System.Collections;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    protected Transform player;

    public int health = 3;
    public float attackRange = 2f;  // فاصله ای که در آن دشمن شروع به حمله می‌کند

    public int attackDamage = -1;
    private bool isAttacking = false; // وضعیت حمله

    public Animator animator; // رفرنس به انیماتور دشمن

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        // محاسبه فاصله از دشمن به بازیکن
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // اگر فاصله کمتر از attackRange باشد، دشمن حمله می‌کند
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
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

        // حرکت دشمن به سمت بازیکن
        transform.position += dir * moveSpeed * Time.deltaTime;

        // محاسبه چرخش به سمت بازیکن
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // چرخش نرم
    }

    protected virtual void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");  // اجرای انیمیشن حمله

            // می‌توانید در اینجا آسیب به بازیکن را نیز وارد کنید
            // مثلا:
            // player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            player.GetComponent<Health>().TakeDamage(attackDamage);
            // مدت زمان حمله را در اینجا تنظیم کنید
            // می‌توانید از یک Coroutine برای صبر کردن بعد از انجام حمله استفاده کنید
            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        // زمان لازم برای پایان انیمیشن حمله (باید مطابق با انیمیشن تنظیم شود)
        yield return new WaitForSeconds(1f);

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
}
