using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array von Asteroiden-Prefabs
    public float minSpawnDelay = 0.5f; // Minimale Wartezeit zwischen Spawns
    public float maxSpawnDelay = 3.0f; // Maximale Wartezeit zwischen Spawns
    public float minYSpawn = -3.0f; // Minimale Y-Position
    public float maxYSpawn = 3.0f; // Maximale Y-Position
    public float spawnXPosition = 11.0f; // X-Position für das Spawnen
    public float asteroidSpeed = 3.0f; // Geschwindigkeit der Asteroiden
    public float destroyXPosition = -11.0f; // X-Position, bei der Asteroiden zerstört werden
    public bool spawnAsteroids = true;

    private void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while (spawnAsteroids)
        {
            // Zufällige Wartezeit zwischen minSpawnDelay und maxSpawnDelay
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);

            SpawnSingleAsteroid();
        }
    }

    private void SpawnSingleAsteroid()
    {
        if (asteroidPrefabs == null || asteroidPrefabs.Length == 0)
        {
            Debug.LogWarning("Keine Asteroiden-Prefabs zugewiesen!");
            return;
        }

        // Wähle zufälliges Prefab und Position
        GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        float spawnYPosition = Random.Range(minYSpawn, maxYSpawn);

        // Spawne Asteroid
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, 0);
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Setze Geschwindigkeit (falls benötigt)
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.left * asteroidSpeed;
        }
    }
}