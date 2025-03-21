using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float movmentSpeed; // Geschwindigkeit der Raumschiffbewegung
    private float speedX, speedY;
    private Rigidbody2D rb;

    // Lebenssystem
    public int maxHealth = 3;
    private int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 2.0f; // Dauer der Unsterblichkeit nach einem Treffer

    // Animation
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Setze die Lebenspunkte auf das Maximum
    }

    // Update is called once per frame
    void Update()
    {
        // Bewegung des Raumschiffs
        speedX = Input.GetAxisRaw("Horizontal") * movmentSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movmentSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Begrenze die Position des Raumschiffs innerhalb des Bildschirms
        if (transform.position.y < -3.4)
        {
            transform.position = new Vector3(transform.position.x, -3.4f, 0);
        }
        else if (transform.position.y > 3.4f)
        {
            transform.position = new Vector3(transform.position.x, 3.4f, 0);
        }

        if (transform.position.x > 8f)
        {
            transform.position = new Vector3(8f, transform.position.y, 0);
        }
        else if (transform.position.x < -8)
        {
            transform.position = new Vector3(-8f, transform.position.y, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Ignoriere Schaden wenn unsterblich

        currentHealth -= damage;
        Debug.Log("Spaceship took damage! Current health: " + currentHealth);

        // Aktiviere Unsterblichkeit für die angegebene Dauer
        StartCoroutine(InvincibilityCooldown());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Coroutine für die Unsterblichkeitsphase
    private IEnumerator InvincibilityCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Spaceship destroyed!");

        //TODO: Game-Over-Logik implementieren
        animator.SetBool("dead", true);
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        // Warte für eine bestimmte Zeit, um die Animation abzuspielen
        yield return new WaitForSeconds(1.0f); // Passen Sie die Zeit an die Länge der Animation an
        
        // Zerstöre das Spielobjekt nach der Animation
        Destroy(gameObject);
    }

    // Kollisionsbehandlung
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.name);

        if (collision.CompareTag("Asteroid"))
        {
            Debug.Log("Asteroid hit!");
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                TakeDamage(asteroid.damage);
            }
        }
    }
}