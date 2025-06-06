using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 10.0f;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        // Zerstöre das Projektil, wenn es den Bildschirm verlässt
        if (transform.position.x > 10)
        {
            Destroy(gameObject);
        }
    }
}