using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;

    public AudioClip jumpSFX;

    public float walkingSpeed = 5.0f;
    public float jumpVelocity = 10.0f;
    public float jumpBufferTime = 0.25f;
    public float wetDuration = 10;
    [HideInInspector] public bool dogLevelComplete = false;
    private bool closeToHuman = false;
    private bool charmingHuman = false;
    [HideInInspector] public bool lockMovement = false;
    private GameObject human;

    AudioSource audioSource;
    Animator animator;
    SpriteRenderer spriteRenderer;

    Rigidbody2D rb2d;

    Vector2 movement;

    public bool wet = false;
    bool jumping;
    bool jumpBuffer;
    bool grounded;
    bool notActive = false;
    public bool swimming = false;

    float x;

    void OnValidate()
    {
        if (groundCheckLeft == null || groundCheckRight == null)
            Debug.LogWarning("At least one player ground check is not assigned!");
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.freezeRotation = true;
    }

    void Update()
    {

        if (!lockMovement)
        {
            if (Input.GetButtonDown("Jump") && grounded || jumpBuffer && grounded && !notActive)
            {
                Jump();
                jumpBuffer = false;
            }

            if (Input.GetButtonDown("Jump") && !grounded && !notActive)
            {
                StartCoroutine(JumpBufferTimer());
            }
        }

        if (closeToHuman && Input.GetButtonDown("Interact") && grounded && !notActive)
        {
            if (human != null)
            {
                if (!charmingHuman && !wet)
                {
                    charmingHuman = true;
                    human.GetComponent<Human>().charmed = true;
                    lockMovement = true;

                } else if (charmingHuman)
                {
                    charmingHuman = false;
                    human.GetComponent<Human>().charmed = false;
                    lockMovement = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        if (!lockMovement && !notActive)
        {
            movement = new Vector2(x * walkingSpeed, rb2d.velocity.y);
            rb2d.velocity = movement;
        }
        CheckIfGrounded();
        CheckJumpForce();
    }

    void Jump()
    {
        if (audioSource != null && jumpSFX != null)
        {
            audioSource.PlayOneShot(jumpSFX);
        }

        if(animator != null)
        {
            //animator.Play(jumpAnimation); //jumpAnimation finns inte än.
        }

        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
        jumping = true;
    }

    IEnumerator JumpBufferTimer()
    {
        jumpBuffer = true;
        yield return new WaitForSecondsRealtime(jumpBufferTime);
        jumpBuffer = false;
    }

    IEnumerator WetFurTimer()
    {
        wet = true;
        yield return new WaitForSecondsRealtime(wetDuration);
        wet = false;
    }

    void CheckIfGrounded()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
           || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            grounded = true;
        }
        else
            grounded = false;
    }

    void CheckJumpForce()
    {
        if (jumping)
        {
            if (!Input.GetButton("Jump"))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2);
                jumping = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            human = other.gameObject;
            closeToHuman = true;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            StartCoroutine(WetFurTimer());
            swimming = true;
        }
        if (other.gameObject.tag == "Finish")
        {
            
            dogLevelComplete = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            closeToHuman = false;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            swimming = false;
        }
        if (other.gameObject.tag == "Finish")
        {
            
            dogLevelComplete = false;
        }
    }
}
