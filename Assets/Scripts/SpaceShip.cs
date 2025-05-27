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

    [Header("Weapons")]
    [SerializeField] private Transform[] weaponMountPoints; // Mount-Punkte für jede Waffe
    [SerializeField] private GameObject[] weaponPrefabs; // Waffen-Prefabs
    private int currentWeaponIndex = 0;
    private GameObject currentWeapon;

    private bool isDashing = false;
    private float lastDashTime;
    private AudioSource audioSource;
    public float movmentSpeed = 5;
    private float speedX, speedY;
    private Rigidbody2D rb;

    public int maxHealth = 3;
    public int currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 2.0f;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        EquipWeapon(currentWeaponIndex); // Starte mit erster Waffe
    }

    void Update()
    {
        // Bewegung
        speedX = Input.GetAxisRaw("Horizontal") * movmentSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movmentSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Dash
        if (Input.GetKeyDown(dashUpKey) && CanDash())
        {
            StartCoroutine(Dash(Vector2.up));
        }
        else if (Input.GetKeyDown(dashDownKey) && CanDash())
        {
            StartCoroutine(Dash(Vector2.down));
        }

        // Bildschirmgrenzen
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -3.4f, 3.4f);
        pos.x = Mathf.Clamp(pos.x, -6f, 6f);
        transform.position = pos;

        // Waffenwechsel
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponPrefabs.Length;
            EquipWeapon(currentWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentWeaponIndex = (currentWeaponIndex - 1 + weaponPrefabs.Length) % weaponPrefabs.Length;
            EquipWeapon(currentWeaponIndex);
        }
    }

    private bool CanDash()
    {
        return !isDashing && Time.time > lastDashTime + dashCooldown;
    }

    private IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        if (dashParticles != null) Instantiate(dashParticles, transform.position, Quaternion.identity);
        if (dashSound != null) audioSource.PlayOneShot(dashSound);

        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + direction * dashDistance;

        GetComponent<Collider2D>().enabled = false;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        GetComponent<Collider2D>().enabled = true;
        isDashing = false;
    }

    private void EquipWeapon(int index)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Mount-Punkt für diese Waffe nutzen, falls vorhanden
        Transform mountPoint = (weaponMountPoints.Length > index) ? weaponMountPoints[index] : transform;

        currentWeapon = Instantiate(weaponPrefabs[index], mountPoint.position, mountPoint.rotation, mountPoint);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log("Spaceship took damage! Current health: " + currentHealth);

        StartCoroutine(InvincibilityCooldown());

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public void Die()
    {
        Debug.Log("Spaceship destroyed!");
        animator.SetBool("dead", true);
        StartCoroutine(WaitForAnimation());

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        PauseMenuUI pauseMenuUI = GetComponent<PauseMenuUI>();
        pauseMenuUI.LoadOtherLevel();
#endif
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.name);

        if (collision.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                TakeDamage(asteroid.damage);
            }
        }
    }
}
