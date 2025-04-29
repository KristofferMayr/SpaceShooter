using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireDelay = 0.5f; // Verzögerung in Sekunden
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Fire Projectile
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            nextFireTime = Time.time + fireDelay;
        }
    }
}
