using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject healthPackPrefab;
    
    [Header("Reward Probabilities")]
    [Range(0, 100)]
    public float nothingChance = 40f;    // 40% chance for nothing
    [Range(0, 100)]
    public float coinChance = 40f;       // 40% chance for coin
    [Range(0, 100)]
    public float healthPackChance = 20f; // 20% chance for health pack
    
    private void OnValidate()
    {
        // Ensure probabilities sum to 100%
        float total = nothingChance + coinChance + healthPackChance;
        if (total != 100f)
        {
            Debug.LogWarning("Reward probabilities should sum to 100%!");
        }
    }
    
    public void SpawnReward(Vector3 position)
    {
        // Random chance for different rewards
        float randomValue = Random.Range(0f, 100f);
        
        if (randomValue < nothingChance) // Nothing
        {
            return;
        }
        else if (randomValue < nothingChance + coinChance) // Coin
        {
            if (coinPrefab != null)
            {
                Instantiate(coinPrefab, position, Quaternion.identity);
            }
        }
        else // Health Pack
        {
            if (healthPackPrefab != null)
            {
                Instantiate(healthPackPrefab, position, Quaternion.identity);
            }
        }
    }
} 