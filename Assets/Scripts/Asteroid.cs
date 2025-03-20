using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
        public float speed = 3.0f;
        public float destroyXPosition = -11.0f;
        public int damage = 1;

        // Update is called once per frame
        void Update()
        {
            // Bewege nach links
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            // Zerstöre Asteroid wenn Bildschirm verlässt
            if (transform.position.x < destroyXPosition)
            {
                Destroy(gameObject);
            }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered with: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Asteroid hit the spaceship!");
            SpaceShip spaceShip = collision.GetComponent<SpaceShip>();
            if (spaceShip != null)
            {
                spaceShip.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit the asteroid!");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}