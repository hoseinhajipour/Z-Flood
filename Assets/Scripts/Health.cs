using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;    // Maximum health
    public int currentHealth;      // Current health
    public bool isDead = false;

    public bool isPlayer = false;  // To distinguish between player and enemy

    // رویدادها برای انیمیشن‌ها
    public event Action OnDamageTaken;
    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth; // Initialize health to max health at the start
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        // فراخوانی رویداد دریافت آسیب
        OnDamageTaken?.Invoke();

        // If health goes below 0, trigger death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle healing (optional)
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    // Method to handle death
    void Die()
    {
        isDead = true;
        // فراخوانی رویداد مرگ
        OnDeath?.Invoke();

        if (isPlayer)
        {
            // Handle player death (you can add game over, respawn logic, etc.)
            Debug.Log("Player died!");

            // غیرفعال کردن کنترل حرکت و شوتینگ
            var controller = GetComponent<PlayerController_>();
            if (controller != null) controller.enabled = false;

            var shooter = GetComponent<AutoShooter>();
            if (shooter != null) shooter.enabled = false;


            // نمایش منوی GameOver
            var gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.ShowGameOver();
        }
        else
        {
            // Handle enemy death (you can add enemy destruction, score, etc.)
            Debug.Log("Enemy died!");

            // Spawn rewards when enemy dies
            RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
            if (rewardSystem != null)
            {
                rewardSystem.SpawnReward(transform.position);
            }

            Destroy(gameObject);  // Destroy the enemy object
        }
    }

    // Optionally, you can show the health as a UI element or visualize it in the editor
    void OnDrawGizmos()
    {
        if (currentHealth <= 0) return;

        // Draw a red bar to represent health in the editor (optional)
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + Vector3.up * 1.5f, new Vector3((float)currentHealth / maxHealth, 0.1f, 0.1f));
    }
}
