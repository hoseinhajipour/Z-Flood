using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;    // Maximum health
    public int currentHealth;      // Current health

    public bool isPlayer = false;  // To distinguish between player and enemy

    void Start()
    {
        currentHealth = maxHealth; // Initialize health to max health at the start
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // If health goes below 0, trigger death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle healing (optional)
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // Clamp to max health
    }

    // Method to handle death
    void Die()
    {
        if (isPlayer)
        {
            // Handle player death (you can add game over, respawn logic, etc.)
            Debug.Log("Player died!");
        }
        else
        {
            // Handle enemy death (you can add enemy destruction, score, etc.)
            Debug.Log("Enemy died!");
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
