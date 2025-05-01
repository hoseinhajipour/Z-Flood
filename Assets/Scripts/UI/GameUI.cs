using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Button upgradeHealthButton;
    public Button upgradeSpeedButton;
    public Button upgradeShootingRangeButton;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerSpeedText;
    public TextMeshProUGUI playerShootingRangeText;
    public GameManager gameManager;

    [Header("Health Bar")]
    public Image healthBarFill;  // نوار پر شونده جان
    public Image healthBarBackground;  // پس‌زمینه نوار جان

    private Health playerHealth; // رفرنس به کامپوننت Health پلیر

    void Start()
    {
        upgradeHealthButton.onClick.AddListener(UpgradeHealth);
        upgradeSpeedButton.onClick.AddListener(UpgradeSpeed);
        upgradeShootingRangeButton.onClick.AddListener(UpgradeShootingRange);

        // پیدا کردن کامپوننت Health پلیر
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        // تنظیم اولیه نوار جان
        if (healthBarFill != null)
        {
            healthBarFill.type = Image.Type.Filled;
            healthBarFill.fillMethod = Image.FillMethod.Horizontal;
            healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }

    void Update()
    {
        scoreText.text = $"{gameManager.score}";
        playerSpeedText.text = $"Speed: {gameManager.playerSpeed:F1}";
        playerShootingRangeText.text = $"Shooting Range: {gameManager.shootingRange:F1}";

        // آپدیت متن و نوار جان از کامپوننت Health
        if (playerHealth != null)
        {
            playerHealthText.text = $"Health: {playerHealth.currentHealth}/{playerHealth.maxHealth}";
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
            }
        }
    }

    void UpgradeHealth()
    {
        if (gameManager.CanUpgrade("health"))
        {
            gameManager.UpgradeHealth();
            // آپدیت جان پلیر در کامپوننت Health
            if (playerHealth != null)
            {
                playerHealth.maxHealth += 10;
                playerHealth.Heal(10);
            }
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
