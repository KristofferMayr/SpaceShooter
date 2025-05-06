using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipBoss : MonoBehaviour
{
    public int health = 500;
    public float speed = 0.1f;
    public int scoreValue = 50; // Punkte pro getötetem Boss
    private bool wasScored = false; // Flag, ob Punkte bereits vergeben wurden
    private float tempMaxSpawnDelay;
    private AsteroidSpawner asteroidSpawner;
    private EnemySpawner enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        tempMaxSpawnDelay = enemySpawner.maxSpawnDelay;
        enemySpawner.maxSpawnDelay = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die(){
        if (!wasScored)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            wasScored = true; // Verhindert doppelte Punktevergabe
        }
        Destroy(gameObject);
        asteroidSpawner.spawnAsteroids = true;  // Nach Bossfigth spawnen wieder Asteroiden
        enemySpawner.maxSpawnDelay = tempMaxSpawnDelay;  // Nach Bossfigth zurück auf normale Spawnrate
    }
}
