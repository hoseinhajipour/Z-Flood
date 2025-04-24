using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyWaveData[] waves;
    public float spawnRadius = 10f;
    public float delayBetweenWaves = 5f;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(WaveManager());
    }

    IEnumerator WaveManager()
    {
        while (true) // حلقه بی‌نهایت برای مدیریت موج‌ها
        {
            yield return StartCoroutine(StartWave(waves[currentWaveIndex]));
            
            // اگر به آخرین موج رسیدیم، همان موج را تکرار کنیم
            if (currentWaveIndex < waves.Length - 1)
            {
                currentWaveIndex++;
            }
            
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    IEnumerator StartWave(EnemyWaveData wave)
    {
        isSpawning = true;
        
        foreach (var entry in wave.enemies)
        {
            if (Random.value <= entry.spawnChance)
            {
                SpawnEnemy(entry.enemyPrefab);
                yield return new WaitForSeconds(delayBetweenWaves); // تأخیر بین اسپان دشمنان
            }
        }
        
        isSpawning = false;
    }

    void SpawnEnemy(GameObject prefab)
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomPoint.x,
            transform.position.y,
            transform.position.z + randomPoint.y
        );

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}