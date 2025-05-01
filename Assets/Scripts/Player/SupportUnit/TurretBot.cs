using UnityEngine;
using System.Collections;

public class TurretBot : MonoBehaviour
{
    [Header("Turret Settings")]
    public float spawnInterval = 10f;
    public int maxTurrets = 2;
    public float turretLifetime = 5f;
    public float spawnRadius = 3f;
    public float minDistanceFromPlayer = 2f;

    [Header("Prefabs")]
    public GameObject turretPrefab;

    private float nextSpawnTime;
    private int currentTurrets;

    void Start()
    {
        currentTurrets = 0;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentTurrets < maxTurrets)
        {
            SpawnTurret();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnTurret()
    {
        // Find a suitable spawn position
        Vector3 spawnPosition = FindSuitableSpawnPosition();
        
        // Create turret
        GameObject turret = Instantiate(turretPrefab, spawnPosition, Quaternion.identity);
        currentTurrets++;

        // Start lifetime countdown
        StartCoroutine(DestroyTurretAfterTime(turret));
    }

    Vector3 FindSuitableSpawnPosition()
    {
        // Find player position using tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag");
            return transform.position;
        }

        Vector3 playerPosition = player.transform.position;
        Vector3 spawnPosition = playerPosition; // Initialize with player position
        int maxAttempts = 10;
        int attempts = 0;
        bool validPosition = false;

        while (!validPosition && attempts < maxAttempts)
        {
            // Get random angle around player
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(minDistanceFromPlayer, spawnRadius);
            
            // Calculate position relative to player
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            spawnPosition = playerPosition + direction * distance;
            
            // Make sure it's on the ground
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, 20f))
            {
                spawnPosition = hit.point;
                
                // Check if position is valid (not too close to player and not in obstacles)
                float distanceToPlayer = Vector3.Distance(spawnPosition, playerPosition);
                if (distanceToPlayer >= minDistanceFromPlayer)
                {
                    // Check if there's enough space around the spawn position
                    Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);
                    if (colliders.Length == 0)
                    {
                        validPosition = true;
                    }
                }
            }
            
            attempts++;
        }

        // If we couldn't find a valid position after max attempts, spawn at a default position
        if (!validPosition)
        {
            Vector3 direction = player.transform.forward;
            spawnPosition = playerPosition + direction * minDistanceFromPlayer;
            
            // Make sure it's on the ground
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, 20f))
            {
                spawnPosition = hit.point;
            }
        }

        return spawnPosition;
    }

    IEnumerator DestroyTurretAfterTime(GameObject turret)
    {
        yield return new WaitForSeconds(turretLifetime);
        
        if (turret != null)
        {
            // Play destruction effect if available
            TurretBotController turretController = turret.GetComponent<TurretBotController>();
            if (turretController != null)
            {
                turretController.PlayDestructionEffect();
            }
            
            Destroy(turret);
            currentTurrets--;
        }
    }

    public void UpgradeTurretBot(float newSpawnInterval, int newMaxTurrets, float newLifetime)
    {
        spawnInterval = newSpawnInterval;
        maxTurrets = newMaxTurrets;
        turretLifetime = newLifetime;
    }

    void OnDrawGizmosSelected()
    {
        // Draw spawn radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        
        // Draw minimum distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromPlayer);
    }
}
