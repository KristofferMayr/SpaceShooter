using System.Collections;
using System.Collections.Generic;
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

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootingInterval = 2f;
    private float nextShootTime;
    public float minShootDistance = 1.5f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        nextDamageTime = Time.time + damageInterval;
        currentHealth = maxHealth;                    // <-- Semikolon!
        nextShootTime = Time.time + shootingInterval; // <-- Semikolon!
    }

    void Update()
    {
        if (player == null) return;

        LookAtPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Bewegung (hier auf 3D umgestellt)
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        // Nahkampfschaden
        if (Time.time >= nextDamageTime && distanceToPlayer < 1f)
        {
            player.GetComponent<SpaceShip>().TakeDamage(damageAmount);
            nextDamageTime = Time.time + damageInterval;
        }

        // Fernkampfangriff
        if (distanceToPlayer >= minShootDistance && Time.time >= nextShootTime)
        {
            ShootAtPlayer();
            nextShootTime = Time.time + shootingInterval;
        }
    }

    void LookAtPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }

    void ShootAtPlayer()
    {
        if (projectilePrefab == null || firePoint == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = dir * 5f;
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (!wasScored)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            wasScored = true;
        }

        if (Random.Range(1, 11) == 10)
        {
            Instantiate(
                powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)],
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }
}
