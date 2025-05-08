using UnityEngine;

public class DragonEnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float damageInterval = 2f;
    public int damageAmount = 1;
    private float nextDamageTime;
    public int scoreValue = 10;
    private bool wasScored = false;
    public GameObject[] powerUpPrefabs;

    public int maxHealth = 3; // Max. Lebenspunkte
    private int currentHealth;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        nextDamageTime = Time.time + damageInterval;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null)
            return;

        LookAtPlayer();

        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Time.time >= nextDamageTime)
        {
            if (Vector2.Distance(transform.position, player.position) < 1f)
            {
                player.GetComponent<SpaceShip>().TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector2 direction = player.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!wasScored)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            wasScored = true;
        }

        // Optional: Spawn Power-Up
        if (Random.Range(1, 11) == 10)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1); // 1 Schaden pro Projektil
        }
    }
}
