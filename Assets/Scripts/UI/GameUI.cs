using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text scoreText;
    public Button upgradeHealthButton;
    public Button upgradeSpeedButton;
    public Button upgradeShootingRangeButton;
    public Text playerHealthText;
    public Text playerSpeedText;
    public Text playerShootingRangeText;
    public GameManager gameManager;

    void Start()
    {
        upgradeHealthButton.onClick.AddListener(UpgradeHealth);
        upgradeSpeedButton.onClick.AddListener(UpgradeSpeed);
        upgradeShootingRangeButton.onClick.AddListener(UpgradeShootingRange);
    }

    void Update()
    {
        scoreText.text = "Score: " + gameManager.score;
        playerHealthText.text = "Health: " + gameManager.playerHealth;
        playerSpeedText.text = "Speed: " + gameManager.playerSpeed;
        playerShootingRangeText.text = "Shooting Range: " + gameManager.shootingRange;
    }

    void UpgradeHealth()
    {
        if (gameManager.CanUpgrade("health"))
        {
            gameManager.UpgradeHealth();
        }
    }

    void UpgradeSpeed()
    {
        if (gameManager.CanUpgrade("speed"))
        {
            gameManager.UpgradeSpeed();
        }
    }

    void UpgradeShootingRange()
    {
        if (gameManager.CanUpgrade("shootingRange"))
        {
            gameManager.UpgradeShootingRange();
        }
    }
}
