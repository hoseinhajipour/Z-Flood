using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public Image healthBarFill;
    
    private Health playerHealth;
    private GameManager gameManager;

    void Start()
    {
        // Find player health component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        // Find game manager
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (playerHealth != null)
        {
            // Update health text and bar
            healthText.text = $"{playerHealth.currentHealth}/{playerHealth.maxHealth}";
            healthBarFill.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        }

        if (gameManager != null)
        {
            // Update score text
            scoreText.text = $"Score: {gameManager.score}";
        }
    }
} 