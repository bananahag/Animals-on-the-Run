using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;
    public AudioClip jumpSFX;
    AudioSource audioSource;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;

    public Material noFrictionMaterial;

    public float walkingSpeed = 5.0f;
    public float jumpVelocity = 10.0f;
    public float jumpBufferTime = 0.25f;
    public float wetDuration = 10;
    private GameObject human;
    private GameObject affectedObject;
    public GameObject objectPlacement;

    Vector2 movement;

    [HideInInspector] public bool dogLevelComplete = false;
    [HideInInspector] public bool lockMovement = false;
    private bool closeToHuman = false;
    private bool charmingHuman = false;
    bool wet = false;
    bool swimming = false;
    bool jumping;
    bool jumpBuffer;
    bool grounded;
   [HideInInspector] public bool notActive = false;
    bool canMoveObject = false;
    bool movingObject = false;
    bool lockJump = false;

    float x;

    void OnValidate()
    {
        if (groundCheckLeft == null || groundCheckRight == null)
            Debug.LogWarning("At least one player ground check is not assigned!");
    }

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
        HandleMovableObjects();
        HandleCharming();
        HandleJumping();

        if (lockMovement)
        {
            rb2d.velocity = new Vector2(0, 0);
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

    void HandleJumping()
    {
        if (!lockMovement && !notActive && !lockJump)
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

    //Hunden kan inte putta objekt utan bara drar.....
    void HandleMovableObjects()
    {
        if (affectedObject != null)
        {
            if (Input.GetButton("Interact") && canMoveObject)
            {
                if (affectedObject != null)
                {
                    affectedObject.transform.parent = gameObject.transform;
                    affectedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    rb2d.isKinematic = true;
                    Physics2D.IgnoreCollision(affectedObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    movingObject = true;
                    lockJump = true;
                }
            }
            else if(!Input.GetButton("Interact") && movingObject)
            {
                affectedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                //canMoveObject = false;

                rb2d.GetComponent<Rigidbody2D>().isKinematic = false;
                affectedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, affectedObject.transform.position.y);
                lockJump = false;
                affectedObject.transform.parent = null;
            }
        }
    }

    void HandleCharming()
    {
        if (closeToHuman && Input.GetButtonDown("Interact") && grounded && !notActive)
        {
            if (human != null)
            {
                if (!charmingHuman && !wet)
                {
                    charmingHuman = true;
                    human.GetComponent<Human>().charmed = true;
                    lockMovement = true;

                }
                else if (charmingHuman)
                {
                    charmingHuman = false;
                    human.GetComponent<Human>().charmed = false;
                    lockMovement = false;
                }
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

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("MoveableObject"))
        {
            
            affectedObject = other.gameObject;

            Vector3 hit = other.contacts[0].normal;
            Debug.Log(hit);
            float angle = Vector3.Angle(hit, Vector3.up);

            if (Mathf.Approximately(angle, 90))
            {
                // Sides
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y > 0)
                { // left side of the player
                    Debug.Log("Left");
                    canMoveObject = true;
                }
                else if (cross.y < 0)
                { // right side of the player
                    Debug.Log("Right");
                    canMoveObject = true;
                    affectedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(rb2d.velocity.x *-100, affectedObject.transform.position.y);

                }
            }
            else
            {
                canMoveObject = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MoveableObject"))
        {
            canMoveObject = false;

                affectedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                rb2d.GetComponent<Rigidbody2D>().isKinematic = false;
                affectedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, affectedObject.transform.position.y);
                lockJump = false;
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
