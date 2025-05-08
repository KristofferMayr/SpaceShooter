using UnityEngine;

public class SpaceshipEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f; // Geschwindigkeit nach links
    [SerializeField] private float retreatXPosition = 6f; // X-Position zum Rückzug
    [SerializeField] private float retreatSpeed = 2f; // Geschwindigkeit beim Rückzug

    [Header("Combat")]
    [SerializeField] private float shootingInterval = 1.5f; // Zeit zwischen Schüssen
    [SerializeField] private EnemyCannon[] cannons; // Array aller Kanonen

    private Transform player;
    private bool hasReachedRetreatPosition = false;
    private float nextShootTime;

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
        if (!hasReachedRetreatPosition)
        {
            // Nach links bewegen
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            // Rückzugsposition prüfen
            if (transform.position.x <= retreatXPosition)
            {
                hasReachedRetreatPosition = true;
            }
        }
        else
        {
            // Rückzug nach rechts (langsam)
            transform.Translate(Vector2.right * retreatSpeed * Time.deltaTime);
        }
    }

    private void HandleShooting()
    {
        if (!hasReachedRetreatPosition) return;

        if (Time.time >= nextShootTime && player != null)
        {
            foreach (var cannon in cannons)
            {
                cannon.Shoot(); // Alle Kanonen feuern
            }
            nextShootTime = Time.time + shootingInterval;
        }
    }
}