using UnityEngine;

public class SpaceShipBoss : MonoBehaviour
{
    [Header("Boss Settings")]
    public int health = 500;
    public float speed = 3f;
    public int scoreValue = 50;
    public float stopXPosition = 4f; // X-Position, an der der Boss stoppt
    public float shootingInterval = 2f; // Zeit zwischen Angriffen

    [Header("References")]
    public GameObject projectilePrefab;
    public Transform[] cannonTransforms; // Array aller Kanonen-Positionen

    private bool wasScored = false;
    private bool hasReachedPosition = false;
    private float nextShootTime;
    private Transform player;
    private AsteroidSpawner asteroidSpawner;
    private SpaceshipEnemySpawner enemySpawner;
    private float tempMaxSpawnDelay;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        enemySpawner = FindObjectOfType<SpaceshipEnemySpawner>();
        tempMaxSpawnDelay = enemySpawner.maxSpawnDelay;
        enemySpawner.maxSpawnDelay = 2.0f;
        nextShootTime = Time.time + shootingInterval;
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        if (!hasReachedPosition)
        {
            // Bewegung nach links zur Zielposition
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(stopXPosition, transform.position.y),
                speed * Time.deltaTime
            );

            // Prüfen ob Zielposition erreicht
            if (Mathf.Abs(transform.position.x - stopXPosition) < 0.1f)
            {
                hasReachedPosition = true;
            }
        }
        else
        {
            // Leichte vertikale Bewegung nach Erreichen der Position
            float newY = Mathf.PingPong(Time.time * 0.5f, 3) - 1.5f; // Bewegt sich zwischen -1.5 und 1.5 auf Y
            transform.position = new Vector2(
                stopXPosition,
                Mathf.Lerp(transform.position.y, newY, Time.deltaTime)
            );
        }
    }

    private void HandleShooting()
    {
        if (!hasReachedPosition) return;

        if (Time.time >= nextShootTime && player != null)
        {
            foreach (Transform cannon in cannonTransforms)
            {
                ShootFromCannon(cannon);
            }
            nextShootTime = Time.time + shootingInterval;
        }
    }

    private void ShootFromCannon(Transform cannon)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            cannon.position,
            Quaternion.Euler(0, 0, -90) // Projektil um -90° gedreht
        );

        Vector2 direction = (player.position - cannon.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 8f;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpaceShip spaceShip = collision.GetComponent<SpaceShip>();
            if (spaceShip != null) spaceShip.Die();
        }

        if (collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }

    private void Die()
    {
        if (!wasScored)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            wasScored = true;
        }
        asteroidSpawner.spawnAsteroids = true;
        enemySpawner.maxSpawnDelay = tempMaxSpawnDelay;
        Destroy(gameObject);
    }
}