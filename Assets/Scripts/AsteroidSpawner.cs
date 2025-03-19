using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array von Asteroiden-Prefabs
    public float spawnRate = 2.0f; // Zeit zwischen den Spawns
    public float minYSpawn = -3.0f; // Minimale Y-Position
    public float maxYSpawn = 3.0f; // Maximale Y-Position
    public float spawnXPosition = 11.0f; // X-Position für das Spawnen
    public float asteroidSpeed = 3.0f; // Geschwindigkeit der Asteroiden
    public float destroyXPosition = -11.0f; // X-Position, bei der Asteroiden zerstört werden

    private void Start()
    {
        // Starte die Spawn-Coroutine
        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            // Warte für die angegebene Spawn-Rate
            yield return new WaitForSeconds(spawnRate);

            // Wähle ein zufälliges Asteroiden-Prefab aus
            GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            float spawnYPosition = Random.Range(minYSpawn, maxYSpawn);

            // Spawne Asteroid
            Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, 0);
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        }
    }
}