using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array von Enemy-Prefabs
    public float minSpawnDelay = 1.0f; // Minimale Wartezeit zwischen Spawns
    public float maxSpawnDelay = 4.0f; // Maximale Wartezeit zwischen Spawns
    public float minYSpawn = -3.0f; // Minimale Y-Position
    public float maxYSpawn = 3.0f; // Maximale Y-Position
    public float minXSpawn = 10.0f; // Minimale X-Position
    public float maxXSpawn = 15.0f; // Maximale X-Position
    public float destroyXPosition = -11.0f; // X-Position, bei der Gegner zerstört werden
    public float difficultyRampRate = 0.1f; // Wie schnell das Spiel schwieriger wird

    private float currentMaxDelay; // Aktuell maximale Verzögerungs

    void Start()
    {
        currentMaxDelay = maxSpawnDelay;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Zufällige Wartezeit mit zunehmendem Schwierigkeitsgrad
            float randomDelay = Random.Range(minSpawnDelay, currentMaxDelay);
            yield return new WaitForSeconds(randomDelay);

            SpawnSingleEnemy();

            // Schwierigkeit langsam erhöhen
            currentMaxDelay = Mathf.Clamp(
                currentMaxDelay - difficultyRampRate * Time.deltaTime, 
                minSpawnDelay, 
                maxSpawnDelay
            );
        }
    }

    private void SpawnSingleEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Keine Enemy-Prefabs im Array zugewiesen!");
            return;
        }

        // Wähle zufälliges Prefab
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Zufällige Position
        float spawnXPosition = Random.Range(minXSpawn, maxXSpawn);
        float spawnYPosition = Random.Range(minYSpawn, maxYSpawn);

        // Spawne Gegner
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, 0);
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 270);
        Instantiate(enemyPrefab, spawnPosition, spawnRotation);
    }
}