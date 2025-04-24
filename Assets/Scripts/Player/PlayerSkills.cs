using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float shootingRange = 8f;
    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // تنظیم مقدار اولیه سلامت
    }

    private void Update()
    {
        // اینجا می‌تونیم هر کاری که بازیکن انجام می‌ده رو بررسی کنیم.
        // به عنوان مثال: حرکت با استفاده از مهارت‌ها
    }

    public void UpgradeMovementSpeed(float amount)
    {
        movementSpeed += amount;
    }

    public void UpgradeShootingRange(float amount)
    {
        shootingRange += amount;
    }

    public void UpgradeHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // وقتی سلامتی ارتقا می‌کنه، سلامت بازیکن رو هم پر می‌کنیم.
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // بازیکن می‌میره، می‌تونیم اینجا اثرات مرگ رو پیاده‌سازی کنیم (مثل نمایش منو یا ریست کردن بازی)
        Debug.Log("Player died!");
    }
}
