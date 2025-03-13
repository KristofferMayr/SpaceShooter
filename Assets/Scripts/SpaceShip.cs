using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float movmentSpeed;
    float speedX, speedY;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * movmentSpeed;
        speedY = Input.GetAxisRaw("Vertical") * movmentSpeed;
        rb.velocity = new Vector2(speedX, speedY);
    }
}
