using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEnemy : MonoBehaviour
{
    public Transform player; // Referenz zum Spieler
    public float moveSpeed = 5f; // Geschwindigkeit, mit der sich der Gegner bewegt
    public float damageInterval = 2f; // Zeitabstand zwischen den Schadenszufügungen
    public int damageAmount = 1; // Menge des Schadens, der zugefügt wird
    private float nextDamageTime; // Zeitpunkt, zu dem der nächste Schaden zugefügt wird
    public int scoreValue = 1; // Punkte pro getötetem Gegner
    private bool wasScored = false; // Flag, ob Punkte bereits vergeben wurden
    public GameObject[] powerUpPrefabs;

    void Start()
    {
        if (player == null)
        {
            // Finde den Spieler automatisch, falls keine Referenz gesetzt ist
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        nextDamageTime = Time.time + damageInterval;
    }

    void Update()
    {
        if (player == null)
            return;

        LookAtPlayer();

        // Bewege dich in Richtung des Spielers
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Überprüfe, ob es Zeit ist, Schaden zuzufügen
        if (Time.time >= nextDamageTime)
        {
            // Überprüfe die Distanz zum Spieler
            if (Vector2.Distance(transform.position, player.position) < 1f) // Annahme: Schaden wird nur zugefügt, wenn der Gegner sehr nah am Spieler ist
            {
                // Füge dem Spieler Schaden zu
                player.GetComponent<SpaceShip>().TakeDamage(damageAmount);

                // Setze den nächsten Zeitpunkt für Schadenszufügung
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    void LookAtPlayer()
    {
        // Berechne die Richtung zum Spieler
        Vector2 direction = player.position - transform.position;
        direction.Normalize();

        // Berechne den Winkel in Grad
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        // Drehe den Gegner in Richtung des Spielers
        transform.rotation = Quaternion.Euler(0, 0, angle);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.name);

        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit the enemy!");
            Destroy(collision.gameObject);
            Die();

            /*
            // Power-Up Spawn Chance (Random Power-Up aus dem Array)
            if(Random.Range(1, 11) == 10){
                Vector3 spawnPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                GameObject.Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)], spawnPosition, Quaternion.identity);
            }
            */
        }
    }
}