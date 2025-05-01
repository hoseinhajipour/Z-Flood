using UnityEngine;
using TapsellPlusSDK;

public class TapSellInitializer : MonoBehaviour
{
    private const string APP_KEY = "ridadigtjjjoeniikijhesbfjapjcseicnjbkqqjrcchgosadelhckqfpprdirdnefbmpr"; // کلید برنامه خود را اینجا قرار دهید

    void Start()
    {
        Debug.Log("Starting TapSell SDK initialization...");
     
        TapsellPlus.Initialize(APP_KEY,
            adNetworkName => 
             {
              Debug.Log(adNetworkName + " Initialized Successfully.");
                // پیدا کردن GameUI و اطلاع دادن به آن
                GameUI gameUI = FindObjectOfType<GameUI>();
                if (gameUI != null)
                {
                    gameUI.OnSDKInitialized();
                }
                else
                {
                    Debug.LogError("GameUI not found in scene!");
                }
            }
            ,
            error => Debug.Log(error.ToString()));
     
     //   TapsellPlus.SetGdprConsent(true);
     /*
        TapsellPlus.Initialize(APP_KEY,
            success =>
            {
                Debug.Log("TapSell SDK initialized successfully: " + success);
                // پیدا کردن GameUI و اطلاع دادن به آن
                GameUI gameUI = FindObjectOfType<GameUI>();
                if (gameUI != null)
                {
                    gameUI.OnSDKInitialized();
                }
                else
                {
                    Debug.LogError("GameUI not found in scene!");
                }
            },
            error =>
            {
                Debug.LogError("TapSell SDK initialization failed: " + error);
                // تلاش مجدد بعد از 30 ثانیه
                Invoke("RetryInitialization", 30f);
            }
        );
        */
    }

    private void RetryInitialization()
    {
        Debug.Log("Retrying TapSell SDK initialization...");
        Start();
    }
}