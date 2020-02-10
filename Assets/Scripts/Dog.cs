using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;
    public AudioClip jumpSFX;
    AudioSource audioSource;
    public Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;

    public Material noFrictionMaterial;

    public float walkingSpeed = 5.0f;
    public float jumpVelocity = 10.0f;
    public float jumpBufferTime = 0.25f;
    public float wetDuration = 10;
    public float timeBetweenStepSounds = 0.5f;

    private GameObject human;
    private GameObject affectedObject;

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
    bool canPlayStepSoundsAgain;
    public bool pushing = false;
    public bool pulling = false;

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
        MovementAnimations();

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
        audioSource.PlayOneShot(jumpSFX);
        animator.Play("Jump");

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

    IEnumerator StepSoundsLoop()
    {
        //audioSource.PlayOneShot(STEP SOUND);
        yield return new WaitForSeconds(timeBetweenStepSounds);
        if (grounded && x != 0)
            canPlayStepSoundsAgain = true;
    }

    void MovementAnimations()
    {
        if(x != 0)
        {
            animator.Play("Walking");
        } else if (x == 0)
        {
            animator.Play("Idle");
        }

        if (canPlayStepSoundsAgain)
        {
            StartCoroutine(StepSoundsLoop());
            canPlayStepSoundsAgain = false;
        }
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

            if (Input.GetButtonDown("Jump") && swimming && !notActive)
            {
                Jump();
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
            if (swimming)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2.2f);
                jumping = false;
            }
        }
    }


    //Hunden ska kunna putta objekt i när den simmar.
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
            float angle = Vector3.Angle(hit, Vector3.up);

            if (Mathf.Approximately(angle, 90))
            {
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y > 0)
                { 
                    canMoveObject = true;
                    
                }
                else if (cross.y < 0)
                {
                    canMoveObject = true;
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
