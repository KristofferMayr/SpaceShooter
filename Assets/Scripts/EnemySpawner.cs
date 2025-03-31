using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public float minSpawnDelay = 1.0f;
    public float maxSpawnDelay = 4.0f;
    public float minYSpawn = -3.0f;
    public float maxYSpawn = 3.0f;
    public float minXSpawn = 10.0f;
    public float maxXSpawn = 15.0f;
    public float destroyXPosition = -11.0f;
    public float difficultyRampRate = 0.1f;

    private float currentMaxDelay;
    private bool bossSpawned = false; // Damit der Boss nur einmal erscheint

    void Start()
    {
        currentMaxDelay = maxSpawnDelay;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float randomDelay = Random.Range(minSpawnDelay, currentMaxDelay);
            yield return new WaitForSeconds(randomDelay);

            SpawnSingleEnemy();

            currentMaxDelay = Mathf.Clamp(
                currentMaxDelay - difficultyRampRate * Time.deltaTime, 
                minSpawnDelay, 
                maxSpawnDelay
            );
        }
    }

    private void SpawnSingleEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        float spawnXPosition = Random.Range(minXSpawn, maxXSpawn);
        float spawnYPosition = Random.Range(minYSpawn, maxYSpawn);

        Instantiate(enemyPrefab, new Vector3(spawnXPosition, spawnYPosition, 0), Quaternion.Euler(0, 0, 270));
    }

    public void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawned) return;
        
        bossSpawned = true;
        Instantiate(bossPrefab, new Vector3(12.14f, 0, 0), Quaternion.Euler(0, 0, -90));
        Debug.Log("Boss Spawned");
    }
}