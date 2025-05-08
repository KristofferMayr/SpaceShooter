using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Transform player; // Referenz zum Spieler
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] public int damageAmount = 1; // Menge des Schadens, der zugefügt wird

    void Start()
    {
        if (player == null)
        {
            // Finde den Spieler automatisch, falls keine Referenz gesetzt ist
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Destroy(gameObject, lifetime); // Automatische Zerstörung nach Zeit
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<SpaceShip>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}