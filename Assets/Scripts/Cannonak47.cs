using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonak47 : MonoBehaviour

{
    public Animator weaponAnimator;
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


        
            if (Input.GetKey(KeyCode.Space))
            {
                weaponAnimator.SetBool("isShooting", true);
            }
            else
            {
                weaponAnimator.SetBool("isShooting", false);
            }
        

        // Fire Projectile
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            nextFireTime = Time.time + fireDelay;
        }
    }
}
