using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array von Enemy-Prefabs
    public float spawnRate = 2.0f; // Zeitabstand zwischen den Spawn-Wellen
    public float minYSpawn = -3.0f; // Minimale Y-Position
    public float maxYSpawn = 3.0f; // Maximale Y-Position
    public float minXSpawn = 10.0f; // Minimale X-Position
    public float maxXSpawn = 15.0f; // Maximale X-Position
    public float destroyXPosition = -11.0f; // X-Position, bei der Gegner zerstört werden

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Warte für die angegebene Spawn-Rate
            yield return new WaitForSeconds(spawnRate);

            // Überprüfe, ob das enemyPrefabs-Array Elemente enthält
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogWarning("Keine Enemy-Prefabs im Array zugewiesen!");
                continue; // Überspringe diese Iteration der Schleife
            }

            // Wähle ein zufälliges Enemy-Prefab aus
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Bestimme eine zufällige X-Position zwischen 10 und 15
            float spawnXPosition = Random.Range(minXSpawn, maxXSpawn);

            // Bestimme eine zufällige Y-Position
            float spawnYPosition = Random.Range(minYSpawn, maxYSpawn);

            // Spawne den Gegner
            Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, 0);
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 270);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);

            spawnRate += (float)0.01;
        }
    }
}