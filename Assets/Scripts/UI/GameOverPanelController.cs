using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanelController : MonoBehaviour
{
    public Button watchAdButton;
    public Button continueButton;
    public Button endGameButton;
    public GameObject panel; // خود پنل GameOver

    private GameManager gameManager;
    private Health playerHealth;
    private PlayerController_ playerController;
    private AutoShooter autoShooter;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerController = player.GetComponent<PlayerController_>();
            autoShooter = player.GetComponent<AutoShooter>();
        }

        if (watchAdButton != null)
            watchAdButton.onClick.AddListener(OnWatchAdButton);
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButton);
        if (endGameButton != null)
            endGameButton.onClick.AddListener(OnEndGameButton);
    }

    // دکمه تماشای ویدئو جایزه‌ای
    public void OnWatchAdButton()
    {
        // نمایش ویدئو جایزه‌ای توسط سیستم تبلیغات (GameUI یا ...)
        // این متد فقط درخواست نمایش ویدئو را می‌دهد
        GameUI gameUI = FindObjectOfType<GameUI>();
        if (gameUI != null)
            gameUI.ShowRewardedAdForContinue();
    }

    // دکمه ادامه با سکه
    public void OnContinueButton()
    {
        if (gameManager != null && gameManager.score >= 500)
        {
            gameManager.score -= 500;
            ResumeGame();
        }
        else
        {
            // نمایش پیام کمبود سکه (اختیاری)
            Debug.Log("Not enough coins to continue!");
        }
    }

    // دکمه پایان بازی
    public void OnEndGameButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("main");
    }

    // منطق مشترک ادامه بازی
    public void ResumeGame()
    {
        if (playerHealth != null)
        {
            playerHealth.currentHealth = 100;
            playerHealth.isDead = false;
        }
        if (playerController != null) playerController.enabled = true;
        if (autoShooter != null) autoShooter.enabled = true;
        if (panel != null) panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnRewardedAdSuccess()
    {
        ResumeGame();
    }
} 