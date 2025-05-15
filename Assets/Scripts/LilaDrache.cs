using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilaDrache : MonoBehaviour
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
    public float minShootDistance = 3f;
    public float maxShootDistance = 6f;

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    private float knockbackEndTime;

    [Header("Behavior Settings")]
    public float attackDuration = 4f;
    public float retreatDuration = 3f;
    public float cooldownFlySpeed = 3f;
    public float flyAmplitude = 1f;
    public float flyFrequency = 1f;

    private enum DragonState { Approaching, Attacking, Retreating, Cooldown }
    private DragonState currentState = DragonState.Approaching;
    private float stateEndTime;
    private bool hasReachedRightSide = false;
    private Vector2 retreatPosition;
    private float originalY;
    private float rightBoundary;
    private Camera mainCamera;

    private Animator animator;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        mainCamera = Camera.main;
        CalculateRightBoundary();

        currentHealth = maxHealth;
        nextShootTime = Time.time + shootingInterval;
        nextDamageTime = Time.time + damageInterval;

        animator = GetComponent<Animator>();
    }

    void CalculateRightBoundary()
    {
        rightBoundary = mainCamera != null
            ? mainCamera.ViewportToWorldPoint(new Vector3(0.85f, 0f, 0f)).x
            : 7f;
    }

    void Update()
    {
        if (player == null) return;

        LookAtPlayer();

        switch (currentState)
        {
            case DragonState.Approaching:
                HandleApproaching();
                break;

            case DragonState.Attacking:
                HandleAttacking();
                break;

            case DragonState.Retreating:
                HandleRetreating();
                break;
        }
    }

    void HandleApproaching()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < maxShootDistance)
        {
            StartAttacking();
        }
        else 
        {
            transform.position = Vector3.MoveTowards(
                 transform.position,
                 player.position,
                 moveSpeed * Time.deltaTime
                 );
        }
    }

    void StartAttacking()
    {
        currentState = DragonState.Attacking;
        stateEndTime = Time.time + attackDuration;

        if (animator != null)
            animator.SetBool("IsDashing", true);
    }

    void HandleAttacking()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * 1.2f * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, player.position) < 1f && Time.time >= nextDamageTime)
        {
            player.GetComponent<SpaceShip>().TakeDamage(damageAmount);
            nextDamageTime = Time.time + damageInterval;
            ApplyKnockback();
            StartRetreating();
        }

        if (Time.time >= stateEndTime)
        {
            StartRetreating();
        }
    }

    void ApplyKnockback()
    {
        isKnockedBack = true;
        knockbackEndTime = Time.time + knockbackDuration;
    }

    void StartRetreating()
    {
        currentState = DragonState.Retreating;
        hasReachedRightSide = false;
        retreatPosition = Vector2.zero;

        if (animator != null)
            animator.SetBool("IsDashing", false);
    }

    void HandleRetreating()
    {
        if (isKnockedBack)
        {
            if (Time.time >= knockbackEndTime)
                isKnockedBack = false;
            else
                transform.Translate(Vector3.right * knockbackForce * Time.deltaTime, Space.World);
        }

        if (!hasReachedRightSide)
        {
            transform.Translate(Vector3.right * cooldownFlySpeed * Time.deltaTime, Space.World);

            if (transform.position.x >= rightBoundary)
            {
                hasReachedRightSide = true;
                retreatPosition = transform.position;
                originalY = retreatPosition.y;
                stateEndTime = Time.time + retreatDuration;
                nextShootTime = Time.time + shootingInterval;
            }
        }
        else
        {
            float newY = originalY + Mathf.Sin(Time.time * flyFrequency) * flyAmplitude;
            transform.position = new Vector3(retreatPosition.x, newY, transform.position.z);

            if (Time.time >= nextShootTime)
            {
                ShootAtPlayer();
                nextShootTime = Time.time + shootingInterval;
            }

            if (Time.time >= stateEndTime)
            {
                currentState = DragonState.Approaching;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle - 270);
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
