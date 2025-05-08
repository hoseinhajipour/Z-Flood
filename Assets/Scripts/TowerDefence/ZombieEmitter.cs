using UnityEngine;
using System.Collections.Generic;

public class ZombieEmitter : MonoBehaviour
{
    [System.Serializable]
    public class ZombieWave
    {
        public GameObject zombiePrefab;
        public int count;
        public float spawnInterval;
        public float delayBeforeNextWave;
    }

    [Header("Wave Settings")]
    public List<ZombieWave> waves = new List<ZombieWave>();
    public Transform[] pathPoints;
    public float pathPointRadius = 0.5f;

    [Header("Spawn Settings")]
    public float spawnRadius = 2f;
    public LayerMask playerLayer;
    public float playerDetectionRange = 10f;

    private int currentWaveIndex = 0;
    private float nextSpawnTime;
    private int zombiesSpawnedInWave;
    private bool isWaveActive = false;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (isWaveActive && Time.time >= nextSpawnTime)
        {
            SpawnZombie();
        }
    }

    void StartNextWave()
    {
        if (currentWaveIndex < waves.Count)
        {
            zombiesSpawnedInWave = 0;
            isWaveActive = true;
            nextSpawnTime = Time.time;
        }
    }

    void SpawnZombie()
    {
        if (currentWaveIndex >= waves.Count) return;

        ZombieWave currentWave = waves[currentWaveIndex];
        
        // Find spawn position
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // Create zombie
        GameObject zombie = Instantiate(currentWave.zombiePrefab, spawnPosition, Quaternion.identity);
        
        // Setup zombie path following
        ZombieAI zombieAI = zombie.GetComponent<ZombieAI>();
        if (zombieAI != null)
        {
            zombieAI.SetPath(pathPoints);
            zombieAI.SetPlayerDetectionRange(playerDetectionRange);
        }

        zombiesSpawnedInWave++;
        nextSpawnTime = Time.time + currentWave.spawnInterval;

        // Check if wave is complete
        if (zombiesSpawnedInWave >= currentWave.count)
        {
            isWaveActive = false;
            currentWaveIndex++;
            Invoke("StartNextWave", currentWave.delayBeforeNextWave);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
        randomPoint.y = 0;
        return transform.position + randomPoint;
    }

    void OnDrawGizmos()
    {
        // Draw spawn radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // Draw path
        if (pathPoints != null && pathPoints.Length > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < pathPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
                Gizmos.DrawWireSphere(pathPoints[i].position, pathPointRadius);
            }
            Gizmos.DrawWireSphere(pathPoints[pathPoints.Length - 1].position, pathPointRadius);
        }
    }
} 