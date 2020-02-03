using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;

    public float walkingSpeed = 5.0f;
    public float jumpVelocity = 10.0f;
    public float jumpBufferTime = 0.25f;

    Rigidbody2D rb2d;

    Vector2 movement;
    bool jumping;
    bool jumpBuffer;
    bool grounded;
    float x;

    void OnValidate()
    {
        if (groundCheckLeft == null || groundCheckRight == null)
            Debug.LogWarning("At least one player ground check is not assigned!");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded || jumpBuffer && grounded)
        {
            Jump();
            jumpBuffer = false;
        }

        if (Input.GetButtonDown("Jump") && !grounded)
        {
            StartCoroutine(JumpBufferTimer());
        }
    }

    void FixedUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");

        movement = new Vector2(x * walkingSpeed, rb2d.velocity.y);
        rb2d.velocity = movement;


        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            grounded = true;
        }
        else
            grounded = false;

        if (jumping)
        {
            if (!Input.GetButton("Jump"))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2);
                jumping = false;
            }
        }

    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
        jumping = true;
    }

    IEnumerator JumpBufferTimer()
    {
        jumpBuffer = true;
        yield return new WaitForSecondsRealtime(jumpBufferTime);
        jumpBuffer = false;
    }
}
