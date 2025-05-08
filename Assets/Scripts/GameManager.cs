using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public int playerHealth;
    public float playerSpeed;
    public float shootingRange;
    public bool canUpgrade;
    public GameObject gameOverPanel;

    void Start()
    {
        score = 0;
        playerHealth = 100;
        playerSpeed = 5f;
        shootingRange = 10f;
    }

    public bool CanUpgrade(string upgradeType)
    {
        // Logic for checking if player can upgrade based on score and other factors
        return score >= 100; // Example: need 100 score to upgrade
    }

    public void UpgradeHealth()
    {
        if (CanUpgrade("health"))
        {
            playerHealth += 10;
            score -= 100;
        }
    }

    public void UpgradeSpeed()
    {
        if (CanUpgrade("speed"))
        {
            playerSpeed += 1f;
            score -= 100;
        }
    }

    public void UpgradeShootingRange()
    {
        if (CanUpgrade("shootingRange"))
        {
            shootingRange += 10f;
            score -= 100;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // توقف زمان بازی (اختیاری)
        Time.timeScale = 0f;
    }
}
