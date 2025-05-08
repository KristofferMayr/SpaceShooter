using UnityEngine;

public class SpaceshipEnemySpawner : EnemySpawner
{
    // Überschreibe die Methode zum Spawnen eines einzelnen Gegners
    protected override void SpawnSingleEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        float spawnXPosition = Random.Range(minXSpawn, maxXSpawn);
        float spawnYPosition = Random.Range(minYSpawn, maxYSpawn);

        // Spawn mit Rotation von 90 Grad (Z-Achse)
        Instantiate(enemyPrefab, new Vector3(spawnXPosition, spawnYPosition, 0), Quaternion.Euler(0, 0, 0));
    }
}
