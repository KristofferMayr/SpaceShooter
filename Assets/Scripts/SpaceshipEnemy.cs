using UnityEngine;

public class EnemySpaceship : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float retreatXPosition = 6f;
    [SerializeField] private float retreatSpeed = 2f;
    [SerializeField] private float maxXPosition = 8f;
    [SerializeField] private float verticalSpeed = 2f;  // Geschwindigkeit auf der Y-Achse
    private float upperY = 3.5f;
    private float lowerY = -3.5f;
    private int verticalDirection = 1;  // 1 = nach oben, -1 = nach unten

    [Header("Combat")]
    [SerializeField] private float shootingInterval = 5f;
    [SerializeField] private EnemyCannon[] cannons;

    private Transform player;
    private bool hasReachedRetreatPosition = false;
    private bool hasReachedMaxX = false;
    private float nextShootTime;
    public int scoreValue = 5; // Punkte pro getötetem Gegner
    private bool wasScored = false; // Flag, ob Punkte bereits vergeben wurden

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextShootTime = Time.time + shootingInterval;
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        // Y-Achsen-Bewegung
        float newY = transform.position.y + verticalSpeed * verticalDirection * Time.deltaTime;
        if (newY >= upperY)
        {
            newY = upperY;
            verticalDirection = -1;
        }
        else if (newY <= lowerY)
        {
            newY = lowerY;
            verticalDirection = 1;
        }

        Vector2 newPosition = new Vector2(transform.position.x, newY);

        if (!hasReachedRetreatPosition)
        {
            // Vorwärtsbewegung nach links
            newPosition.x = transform.position.x - moveSpeed * Time.deltaTime;

            if (newPosition.x <= retreatXPosition)
            {
                hasReachedRetreatPosition = true;
                hasReachedMaxX = false;
            }
        }
        else if (!hasReachedMaxX)
        {
            // Rückzug nach rechts
            newPosition.x = Mathf.Min(
                transform.position.x + retreatSpeed * Time.deltaTime,
                maxXPosition
            );

            if (newPosition.x >= maxXPosition)
            {
                hasReachedMaxX = true;
                hasReachedRetreatPosition = false;
            }
        }

        transform.position = newPosition;
    }

    private void HandleShooting()
    {
        if (hasReachedRetreatPosition && !hasReachedMaxX)
        {
            if (Time.time >= nextShootTime)
            {
                foreach (var cannon in cannons)
                {
                    cannon.Shoot();
                }
                nextShootTime = Time.time + shootingInterval;
            }
        }
    }

    void Die()
    {

        if (!wasScored)
        {
            ScoreManager.Instance.AddScore(scoreValue);
            wasScored = true; // Verhindert weitere Punktevergabe
        }
        
        // Gegner zerstören
        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.name);

        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit the enemy!");
            Destroy(collision.gameObject);
            Die();
        }
    }
}