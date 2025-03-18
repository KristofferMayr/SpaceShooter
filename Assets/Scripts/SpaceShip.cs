using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float movmentSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;

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
        
        // Player Boundries
	    if (transform.position.y < -13){
            transform.position = new Vector3(transform.position.x, -13f, 0);
        }
        else if (transform.position.y > 13f){
            transform.position = new Vector3(transform.position.x, 13f, 0);
        }

        if (transform.position.x > 31.58f){
            transform.position = new Vector3(31.58f, transform.position.y, 0);
        }
        else if (transform.position.x < -31.58f){
            transform.position = new Vector3(-31.58f, transform.position.y, 0);
        }
    }

    private void shoot(){
        
    }
}
