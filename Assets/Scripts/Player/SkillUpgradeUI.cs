using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradeUI : MonoBehaviour
{
    public Text movementSpeedText;
    public Text shootingRangeText;
    public Text healthText;

    private PlayerSkills playerSkills;

    void Start()
    {
        playerSkills = FindObjectOfType<PlayerSkills>();
    }

    void Update()
    {
        if (playerSkills != null)
        {
            movementSpeedText.text = "Speed: " + playerSkills.movementSpeed.ToString("F1");
            shootingRangeText.text = "Range: " + playerSkills.shootingRange.ToString("F1");
            healthText.text = "Health: " + playerSkills.currentHealth + "/" + playerSkills.maxHealth;
        }
    }

    public void OnSpeedUpgrade()
    {
        playerSkills.UpgradeMovementSpeed(1f);
    }

    public void OnRangeUpgrade()
    {
        playerSkills.UpgradeShootingRange(2f);
    }

    public void OnHealthUpgrade()
    {
        playerSkills.UpgradeHealth(20);
    }
}
