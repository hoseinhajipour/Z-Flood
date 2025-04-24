using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Button buyHealthPackButton;
    public Button buySpeedPackButton;
    public Button watchAdForLifeButton;
    public GameManager gameManager;

    void Start()
    {
        buyHealthPackButton.onClick.AddListener(BuyHealthPack);
        buySpeedPackButton.onClick.AddListener(BuySpeedPack);
        watchAdForLifeButton.onClick.AddListener(WatchAdForLife);
    }

    void BuyHealthPack()
    {
        if (gameManager.score >= 200)
        {
            gameManager.playerHealth += 20;
            gameManager.score -= 200;
        }
    }

    void BuySpeedPack()
    {
        if (gameManager.score >= 150)
        {
            gameManager.playerSpeed += 2f;
            gameManager.score -= 150;
        }
    }

    void WatchAdForLife()
    {
        // Code for watching ad and rewarding extra life
        StartCoroutine(WatchAdCoroutine());
    }

    private System.Collections.IEnumerator WatchAdCoroutine()
    {
        // Simulate watching an ad
        yield return new WaitForSeconds(5f); // Simulating the ad duration
        gameManager.playerHealth += 50; // Reward the player with extra life
    }
}
