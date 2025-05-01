using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the GameManager and add score
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(coinValue);
            }
            
            // Destroy the coin
            Destroy(gameObject);
        }
    }
} 