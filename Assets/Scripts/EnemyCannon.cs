using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Transform firePoint; // Wo das Projektil spawnen soll

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip shootSound;

    private float nextFireTime;
    private AudioSource audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        if (firePoint == null) firePoint = transform; // Fallback
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    public void Shoot()
    {
        // Projektil instanziieren
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position, Quaternion.Euler(0, 0, 0)
        );

        // Projektil nach links bewegen
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.left * projectileSpeed;
        }

        // Effekte
        //if (muzzleFlash != null) muzzleFlash.Play();
        //if (shootSound != null) audioSource.PlayOneShot(shootSound);
    }
}