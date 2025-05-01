using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class SupportUnit
{
    public string unitName;
    public GameObject prefab;
    public int price;
    public Sprite icon;
    [HideInInspector] public bool isPurchased = false;
    [HideInInspector] public Button buyButton;
    [HideInInspector] public TextMeshProUGUI priceText;
}

public class ShopUI : MonoBehaviour
{
    public GameManager gameManager;
    
    [Header("UI Elements")]
    public GameObject supportUnitButtonPrefab; // Prefab for the button with icon and price
    public Transform supportUnitsContainer; // Parent transform for the buttons
    
    [Header("Support Units")]
    public List<SupportUnit> supportUnits = new List<SupportUnit>();

    void Start()
    {
        CreateSupportUnitButtons();
    }

    void CreateSupportUnitButtons()
    {
        // Clear existing buttons
        foreach (Transform child in supportUnitsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each support unit
        foreach (var unit in supportUnits)
        {
            // Instantiate button prefab
            GameObject buttonObj = Instantiate(supportUnitButtonPrefab, supportUnitsContainer);
            
            // Get button component
            Button button = buttonObj.GetComponent<Button>();
            unit.buyButton = button;
            
            // Set button icon
            Image iconImage = buttonObj.transform.Find("Icon").GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = unit.icon;
            }
            
            // Set price text
            TextMeshProUGUI priceText = buttonObj.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
            if (priceText != null)
            {
                priceText.text = $"Price: {unit.price}";
                unit.priceText = priceText;
            }
            
            // Set unit name text
            TextMeshProUGUI nameText = buttonObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            if (nameText != null)
            {
                nameText.text = unit.unitName;
            }
            
            // Add click listener
            button.onClick.AddListener(() => BuySupportUnit(unit));
        }

        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        foreach (var unit in supportUnits)
        {
            if (unit.buyButton != null)
            {
                unit.buyButton.interactable = !unit.isPurchased && gameManager.score >= unit.price;
            }
        }
    }

    void BuySupportUnit(SupportUnit unit)
    {
        if (!unit.isPurchased && gameManager.score >= unit.price)
        {
            gameManager.score -= unit.price;
            unit.isPurchased = true;
            unit.buyButton.interactable = false;
            SpawnSupportUnit(unit.prefab);
            UpdateButtonStates();
        }
    }

    void SpawnSupportUnit(GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 spawnPosition = player.transform.position + new Vector3(2f, 0f, 0f);
                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        UpdateButtonStates();
    }
}
