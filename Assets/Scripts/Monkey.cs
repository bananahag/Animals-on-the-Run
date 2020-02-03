using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float walkingSpeed = 5.0f;
    public float jumpForce = 500.0f;

    Vector2 movement;
    float x;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");

        movement = new Vector2(x * walkingSpeed, rb2d.velocity.y);
        rb2d.velocity = movement;
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
        rb2d.AddForce(new Vector2(0.0f, jumpForce));
        while (rb2d.velocity.y > 0)
        {
            print("AAAAAAAAAAAAAAAAAAAAAAAH");
            if (!Input.GetButton("Jump"))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2);
                break;
            }
        }
    }
}
