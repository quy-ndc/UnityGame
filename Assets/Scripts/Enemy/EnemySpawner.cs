using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroup; // List of groups of enemy to spawn
        public int waveQuota;       // Max number of spawns in this wave
        public float spawnInterval; // The interval to spawn enemy
        public int spawnCount;      // Number of current spawns
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;        // List of all waves in the game
    public int currentWaveCount;    // The index of the current wave

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEmemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval; 

    [Header("Spawn positions")]
    public List<Transform> relativeSpawnPoints;

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if wave ended and start next wave
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn the next enemy
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    IEnumerator BeginNextWave()
    {
        // Wave for 'Wave Interval' seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);
        
        // If there are more waves to start after the current wave, move on to the next wave
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroup)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }


    void SpawnEnemy()
    {
        // Check if the minimun number of enemies in the wave have beeen spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            // Spawn each type of enemy until it reaches the quota
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroup)
            {
                // Check if the minimun number of enemies of this type have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    // Limit the number of enemies that can be spawned at once
                    if (enemiesAlive >= maxEmemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    // Spawn enemies randomly close to player
                    Instantiate(enemyGroup.enemyPrefab, 
                                player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, 
                                Quaternion.identity);

                    //Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f),
                    //                                    player.transform.position.y + Random.Range(-10f, 10f));
                    //Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        if (enemiesAlive < maxEmemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
