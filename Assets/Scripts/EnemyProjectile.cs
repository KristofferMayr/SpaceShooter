using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float rotationOffset = -90f; // Konstante Drehung

    private Transform player;
    private Rigidbody2D rb;

    public int getDamage(){
        return damage;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Finde den Spieler automatisch
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (player != null)
        {
            // Initiale Drehung auf -90 Grad setzen
            transform.rotation = Quaternion.Euler(0, 0, rotationOffset);
            
            // Bewegung initialisieren
            InitializeMovement();
            Destroy(gameObject, lifetime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMovement()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;

        // Winkelberechnung mit Offset
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotationOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            SpaceShip playerShip = other.GetComponent<SpaceShip>();
            if (playerShip != null)
            {
                playerShip.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}