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

    [System.Serializable]
    public class Path
    {
        public Transform[] points;
    }

    [Header("Path Settings")]
    public Transform mainPath; // The main node containing all path branches
    public List<Path> multiPaths = new List<Path>(); // List of all paths

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

        // Instantiate zombie
        GameObject zombie = Instantiate(currentWave.zombiePrefab, spawnPosition, Quaternion.identity);

        // Select a path randomly from multiPaths
        Transform[] selectedPath = null;
        if (multiPaths != null && multiPaths.Count > 0)
        {
            int pathIndex = Random.Range(0, multiPaths.Count);
            selectedPath = multiPaths[pathIndex].points;
        }

        // Set up zombie path following and NavMesh
        ZombieAI zombieAI = zombie.GetComponent<ZombieAI>();
        if (zombieAI != null && selectedPath != null)
        {
            zombieAI.SetPath(selectedPath);
            zombieAI.SetPlayerDetectionRange(playerDetectionRange);
            var agent = zombie.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
                agent.enabled = true;
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

        // Draw all paths in multiPaths
        if (multiPaths != null && multiPaths.Count > 0)
        {
            Gizmos.color = Color.yellow;
            foreach (var path in multiPaths)
            {
                if (path != null && path.points != null && path.points.Length > 1)
                {
                    for (int i = 0; i < path.points.Length - 1; i++)
                    {
                        Gizmos.DrawLine(path.points[i].position, path.points[i + 1].position);
                        Gizmos.DrawWireSphere(path.points[i].position, pathPointRadius);
                    }
                    Gizmos.DrawWireSphere(path.points[path.points.Length - 1].position, pathPointRadius);
                }
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Auto Fill MultiPaths From MainPath")]
    public void AutoFillMultiPaths()
    {
        multiPaths.Clear();
        if (mainPath == null) return;

        foreach (Transform child in mainPath)
        {
            Path path = new Path();
            List<Transform> points = new List<Transform>();
            foreach (Transform point in child)
            {
                points.Add(point);
            }
            path.points = points.ToArray();
            multiPaths.Add(path);
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}