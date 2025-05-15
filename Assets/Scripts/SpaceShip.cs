using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private KeyCode dashUpKey = KeyCode.Q;
    [SerializeField] private KeyCode dashDownKey = KeyCode.E;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem dashParticles;
    [SerializeField] private AudioClip dashSound;
    private bool isDashing = false;
    private float lastDashTime;
    private AudioSource audioSource;
    public float movmentSpeed = 5; // Geschwindigkeit der Raumschiffbewegung
    private float speedX, speedY;
    private Rigidbody2D rb;

    // Lebenssystem
    public int maxHealth = 3;
    public int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 2.0f; // Dauer der Unsterblichkeit nach einem Treffer

    // Animation
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Setze die Lebenspunkte auf das Maximum
        audioSource = GetComponent<AudioSource>();
    }

    private bool CanDash()
    {
        return !isDashing && Time.time > lastDashTime + dashCooldown;
    }

    private IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Effekte starten
        if (dashParticles != null) Instantiate(dashParticles, this.transform.position, Quaternion.identity);;
        if (dashSound != null) audioSource.PlayOneShot(dashSound);

        // Originalposition speichern
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction * dashDistance;

        // Kollision während Dash deaktivieren
        GetComponent<Collider2D>().enabled = false;

        // Dash-Bewegung
        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Finale Position setzen
        transform.position = endPos;

        // Kollision wieder aktivieren
        GetComponent<Collider2D>().enabled = true;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Bewegung des Raumschiffs
        speedX = Input.GetAxisRaw("Horizontal") * movmentSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movmentSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Dash mit WASD + Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.W) && CanDash())
            {
                StartCoroutine(Dash(Vector2.up));
            }
            else if (Input.GetKeyDown(KeyCode.S) && CanDash())
            {
                StartCoroutine(Dash(Vector2.down));
            }
            else if (Input.GetKeyDown(KeyCode.A) && CanDash())
            {
                StartCoroutine(Dash(Vector2.left));
            }
            else if (Input.GetKeyDown(KeyCode.D) && CanDash())
            {
                StartCoroutine(Dash(Vector2.right));
            }
        }

        // Begrenze die Position des Raumschiffs innerhalb des Bildschirms
        if (transform.position.y < -3.4f)
        {
            transform.position = new Vector3(transform.position.x, -3.4f, 0);
        }
        else if (transform.position.y > 3.4f)
        {
            transform.position = new Vector3(transform.position.x, 3.4f, 0);
        }

        if (transform.position.x > 6f)
        {
            transform.position = new Vector3(6f, transform.position.y, 0);
        }
        else if (transform.position.x < -6f)
        {
            transform.position = new Vector3(-6f, transform.position.y, 0);
        }
    }


    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Ignoriere Schaden wenn unsterblich

        currentHealth -= damage;
        Debug.Log("Spaceship took damage! Current health: " + currentHealth);

        // Aktiviere Unsterblichkeit für die angegebene Dauer
        StartCoroutine(InvincibilityCooldown());

        if (currentHealth == 0)
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

    public void Die()
    {
        Debug.Log("Spaceship destroyed!");

        //TODO: Game-Over-Logik implementieren -> Restart, Level-Overview usw.
        animator.SetBool("dead", true);
        StartCoroutine(WaitForAnimation());

        // Application Quit funktioniert nur im build deshalb: (# um während kompilierungszeit ausgwertet zu werden)
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //im editor
        #else
            PauseMenuUI pauseMenuUI = GetComponent<PauseMenuUI>();
            pauseMenuUI.LoadOtherLevel();
        #endif
    }

    private IEnumerator WaitForAnimation()
    {
        Debug.Log("WAIT ANIMATION");
        // Warte für eine bestimmte Zeit, um die Animation abzuspielen
        yield return new WaitForSeconds(5.0f);
        
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