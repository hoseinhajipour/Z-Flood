using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerPlacementSystem : MonoBehaviour
{
    [Header("Tower Prefabs")]
    public GameObject machineGunTowerPrefab;
    public GameObject artilleryTowerPrefab;
    public GameObject laserTowerPrefab;
    public GameObject flamethrowerTowerPrefab;

    [Header("UI")]
    public GameObject towerMenu;
    public Button machineGunButton;
    public Button artilleryButton;
    public Button laserButton;
    public Button flamethrowerButton;
    public Text coinsText;

    [Header("Settings")]
    public LayerMask placementLayer;
    public float placementRadius = 1f;
    public int startingCoins = 1000;

    private int currentCoins;
    private GameObject selectedTower;
    private bool isPlacingTower = false;
    private List<GameObject> placedTowers = new List<GameObject>();

    void Start()
    {
        currentCoins = startingCoins;
        UpdateCoinsUI();

        // Setup button listeners
        machineGunButton.onClick.AddListener(() => StartPlacingTower(machineGunTowerPrefab));
        artilleryButton.onClick.AddListener(() => StartPlacingTower(artilleryTowerPrefab));
        laserButton.onClick.AddListener(() => StartPlacingTower(laserTowerPrefab));
        flamethrowerButton.onClick.AddListener(() => StartPlacingTower(flamethrowerTowerPrefab));
    }

    void Update()
    {
        if (isPlacingTower)
        {
            UpdateTowerPosition();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    void StartPlacingTower(GameObject towerPrefab)
    {
        BaseTower towerComponent = towerPrefab.GetComponent<BaseTower>();
        if (towerComponent != null && currentCoins >= towerComponent.cost)
        {
            selectedTower = Instantiate(towerPrefab);
            isPlacingTower = true;
            towerMenu.SetActive(false);
        }
    }

    void UpdateTowerPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
        {
            selectedTower.transform.position = hit.point;
        }
    }

    void PlaceTower()
    {
        if (CanPlaceTower())
        {
            BaseTower towerComponent = selectedTower.GetComponent<BaseTower>();
            currentCoins -= towerComponent.cost;
            UpdateCoinsUI();
            placedTowers.Add(selectedTower);
            isPlacingTower = false;
            selectedTower = null;
        }
    }

    bool CanPlaceTower()
    {
        if (selectedTower == null) return false;

        // Check if too close to other towers
        foreach (var tower in placedTowers)
        {
            if (Vector3.Distance(selectedTower.transform.position, tower.transform.position) < placementRadius * 2)
            {
                return false;
            }
        }

        return true;
    }

    void CancelPlacement()
    {
        if (selectedTower != null)
        {
            Destroy(selectedTower);
        }
        isPlacingTower = false;
        towerMenu.SetActive(true);
    }

    void UpdateCoinsUI()
    {
        coinsText.text = "Coins: " + currentCoins;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinsUI();
    }
} 