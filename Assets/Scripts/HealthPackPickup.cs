using UnityEngine;

public class HealthPackPickup : MonoBehaviour
{
    public int healAmount = 20;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the player's health component
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }
            
            // Destroy the health pack
            Destroy(gameObject);
        }
    }
} 