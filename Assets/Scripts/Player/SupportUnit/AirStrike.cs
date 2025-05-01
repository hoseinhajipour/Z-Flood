using UnityEngine;
using System.Collections;

public class AirStrike : MonoBehaviour
{
    [Header("Strike Settings")]
    public float strikeInterval = 10f;
    public int damage = 30;
    public float strikeRadius = 5f;
    public int maxTargets = 3;
    public float missileSpeed = 20f;
    public float missileHeight = 50f;

    [Header("Prefabs")]
    public GameObject missilePrefab;
    public GameObject explosionPrefab;
    public GameObject targetIndicatorPrefab;

    private float nextStrikeTime;
    private GameObject[] targetIndicators;

    void Start()
    {
        // Initialize target indicators
        targetIndicators = new GameObject[maxTargets];
        for (int i = 0; i < maxTargets; i++)
        {
            targetIndicators[i] = Instantiate(targetIndicatorPrefab);
            targetIndicators[i].SetActive(false);
        }
    }

    void Update()
    {
        if (Time.time >= nextStrikeTime)
        {
            FindAndMarkTargets();
            nextStrikeTime = Time.time + strikeInterval;
        }
    }

    void FindAndMarkTargets()
    {
        // Find all enemies in a large radius
        Collider[] enemies = Physics.OverlapSphere(transform.position, 50f);
        int targetCount = 0;

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && targetCount < maxTargets)
            {
                // Activate and position target indicator
                targetIndicators[targetCount].SetActive(true);
                targetIndicators[targetCount].transform.position = enemy.transform.position + Vector3.up * 0.1f;
                
                // Start missile strike sequence
                StartCoroutine(LaunchMissile(targetIndicators[targetCount].transform.position));
                
                targetCount++;
            }
        }

        // Deactivate unused indicators
        for (int i = targetCount; i < maxTargets; i++)
        {
            targetIndicators[i].SetActive(false);
        }
    }

    IEnumerator LaunchMissile(Vector3 targetPosition)
    {
        // Create missile at high altitude
        Vector3 startPosition = targetPosition + new Vector3(0, missileHeight, 0);
        GameObject missile = Instantiate(missilePrefab, startPosition, Quaternion.identity);
        
        // Calculate direction to target
        Vector3 direction = (targetPosition - startPosition).normalized;
        missile.transform.forward = direction;

        // Move missile towards target
        float distance = Vector3.Distance(startPosition, targetPosition);
        float timeToTarget = distance / missileSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < timeToTarget)
        {
            missile.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / timeToTarget);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Create explosion at target
        GameObject explosion = Instantiate(explosionPrefab, targetPosition, Quaternion.identity);
        
        // Deal damage to enemies in radius
        Collider[] hitEnemies = Physics.OverlapSphere(targetPosition, strikeRadius);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.TakeDamage(damage);
                }
            }
        }

        // Clean up
        Destroy(missile);
        Destroy(explosion, 2f);
    }

    public void UpgradeAirStrike(float newStrikeInterval, int damageIncrease, float radiusIncrease)
    {
        strikeInterval = newStrikeInterval;
        damage += damageIncrease;
        strikeRadius += radiusIncrease;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 50f); // Search radius
    }
}
