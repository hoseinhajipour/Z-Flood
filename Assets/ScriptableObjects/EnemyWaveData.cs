using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/Wave Data", order = 1)]
public class EnemyWaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject enemyPrefab;
        [Range(0, 1)] public float spawnChance = 1f;
        public float spawnDelay = 0.5f; // تأخیر بین اسپان دشمنان
    }

    public EnemyEntry[] enemies;
}