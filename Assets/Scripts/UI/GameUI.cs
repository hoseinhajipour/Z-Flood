using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapsellPlusSDK;

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

    [Header("Rewarded Ad")]
    public Button watchAdButton;  // دکمه تماشای تبلیغ
    public TextMeshProUGUI adRewardText;  // متن جایزه تبلیغ

    private Health playerHealth; // رفرنس به کامپوننت Health پلیر
    private string _responseId; // شناسه پاسخ تبلیغ
    private const string ZONE_ID = "681325a6e18d5645456c1254"; // شناسه تبلیغگاه خود را اینجا قرار دهید
    private bool _isInitialized = false;

    void Start()
    {
        upgradeHealthButton.onClick.AddListener(UpgradeHealth);
        upgradeSpeedButton.onClick.AddListener(UpgradeSpeed);
        upgradeShootingRangeButton.onClick.AddListener(UpgradeShootingRange);
        watchAdButton.onClick.AddListener(WatchRewardedAd);


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

        // صبر می‌کنیم تا SDK مقداردهی اولیه شود
        Invoke("RequestRewardedAd", 2f);
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

    private void RequestRewardedAd()
    {
        if (!_isInitialized)
        {
            Debug.Log("SDK not initialized yet, retrying in 2 seconds...");
            Invoke("RequestRewardedAd", 2f);
            return;
        }

        Debug.Log("Requesting rewarded ad...");
        TapsellPlus.RequestRewardedVideoAd(ZONE_ID,
            tapsellPlusAdModel =>
            {
                Debug.Log("Ad request successful: " + tapsellPlusAdModel.responseId);
                _responseId = tapsellPlusAdModel.responseId;
            },
            error =>
            {
                Debug.LogError("Ad request failed: " + error);
                Invoke("RequestRewardedAd", 30f);
            }
        );
    }

    void WatchRewardedAd()
    {
        if (!_isInitialized)
        {
            Debug.Log("SDK not initialized yet");
            return;
        }

        if (string.IsNullOrEmpty(_responseId))
        {
            Debug.Log("Ad is not ready yet. Requesting a new ad...");
            RequestRewardedAd();
            return;
        }

        Debug.Log("Showing ad with response ID: " + _responseId);
        TapsellPlus.ShowRewardedVideoAd(_responseId,
            tapsellPlusAdModel =>
            {
                Debug.Log("Ad opened: " + tapsellPlusAdModel.zoneId);
            },
            tapsellPlusAdModel =>
            {
                Debug.Log("Reward granted: " + tapsellPlusAdModel.zoneId);
                gameManager.AddScore(100);
                Debug.Log("Player rewarded with 100 coins");
                // تنظیم متن جایزه تبلیغ
                if (adRewardText != null)
                {
                    adRewardText.text = "+100 Coins";
                }

            },
            tapsellPlusAdModel =>
            {
                Debug.Log("Ad closed: " + tapsellPlusAdModel.zoneId);
                RequestRewardedAd();
            },
            error =>
            {
                Debug.LogError("Ad show error: " + error);
                Invoke("RequestRewardedAd", 30f);
            }
        );
    }

    // این متد توسط TapSellInitializer فراخوانی می‌شود
    public void OnSDKInitialized()
    {
        _isInitialized = true;
        Debug.Log("GameUI: SDK initialized successfully");
        RequestRewardedAd();
    }

    public void ShowRewardedAdForContinue()
    {
        if (!_isInitialized)
        {
            Debug.Log("SDK not initialized yet");
            return;
        }

        if (string.IsNullOrEmpty(_responseId))
        {
            Debug.Log("Ad is not ready yet. Requesting a new ad...");
            RequestRewardedAd();
            return;
        }

        Debug.Log("Showing ad for continue with response ID: " + _responseId);
        TapsellPlus.RequestRewardedVideoAd(ZONE_ID,
            tapsellPlusAdModel =>
            {
                _responseId = tapsellPlusAdModel.responseId;
                TapsellPlus.ShowRewardedVideoAd(_responseId,
                    adModel => { Debug.Log("Ad opened: " + adModel.zoneId); },
                    adModel =>
                    {
                        Debug.Log("Reward granted: " + adModel.zoneId);
                        var gameOverPanel = FindObjectOfType<GameOverPanelController>();
                        if (gameOverPanel != null)
                            gameOverPanel.OnRewardedAdSuccess();
                    },
                    adModel =>
                    {
                        Debug.Log("Ad closed: " + adModel.zoneId);
                        RequestRewardedAd();
                    },
                    error =>
                    {
                        Debug.LogError("Ad show error: " + error);
                        Invoke("RequestRewardedAd", 30f);
                    }
                );
            },
            error =>
            {
                Debug.LogError("Ad request failed: " + error);
                Invoke("RequestRewardedAd", 30f);
            }
        );
    }
}
