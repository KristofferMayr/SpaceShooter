using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMorter : EnemyCannon
{
    protected override void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}