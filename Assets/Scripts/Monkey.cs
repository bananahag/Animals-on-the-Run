using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;
    public AudioClip jumpSFX, scaredSFX;//+ more sound effects

    public float walkingSpeed = 5.0f;
    public float walkingSpeedWhenCarryingBucket = 3.5f;
    public float timeBetweenStepSounds = 0.5f;
    public float walkAwayScaredTime = 0.5f;

    public float climbingSpeed = 4.0f;
    public float timeBetweenClimbingSounds = 0.5f;
    public float climbingOffsetFromCenterDistance = 0.25f;
    public float stopClimbingTime = 0.5f;
    public float canClimbAgainAfterJumpingTime = 0.25f;

    public float jumpVelocity = 10.0f;
    public float jumpBufferTime = 0.25f;
    public bool CurrentChar;

    public float timeStoppedWhenPickingUpEel = 1.0f;
    public float timeStoppedWhenActivating = 1.0f;

    [HideInInspector]
    public bool cannotMove;
    [HideInInspector]
    public bool notActive;

    AudioSource audioSource;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;

    Vector2 movement;
    float x, y;
    bool facingRight;
    
    bool jumping;
    bool jumpBuffer;

    bool grounded;
    bool canPlayStepSoundsAgain;

    bool canPickUpEel;
    bool carrying;

    bool canClimb, canClimbAfterJumping;
    bool climbing;
    bool canPlayClimbingSoundAgain, canChangeClimbingDirection;

    bool canOpenCage;
    bool canPullLever, canPushButton;
    [HideInInspector]
    public bool monkeyLevelComplete = false;
    GameObject lever, button;
    GameObject cage;
    GameObject scaryObject; //water or a human

    float ladderXPosition;
    float startGravityScale;

    Color startingColor; //REMOVE THIS WHEN YOU HAVE THE FINAL MONKEY ANIMATIONS!!!!!!!!!!!!

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

        startGravityScale = rb2d.gravityScale;
        canPlayClimbingSoundAgain = true;
        canClimbAfterJumping = true;

        startingColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded && !carrying && !cannotMove && !notActive || jumpBuffer && grounded && !carrying && !cannotMove && !notActive
            || Input.GetButtonDown("Jump") && climbing && !carrying && !cannotMove && !notActive)
        {
            Jump();
            jumpBuffer = false;
        }

        if (Input.GetButtonDown("Jump") && !grounded && !cannotMove && !notActive)
        {
            StartCoroutine(JumpBufferTimer());
        }

        if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
        {
            if (!carrying && canPickUpEel)
            {
                //animator.Play(PICK UP EEL);
                //audioSource.PlayOneShot(EEL PICKUP SFX);
                StartCoroutine(StopToPickUpEel());
                spriteRenderer.color = Color.cyan;
                carrying = true;
            }
            else if (carrying)
            {
                //animator.Play(DROP EEL);
                //audioSource.PlayOneShot(EEL DROP SFX);
                //Spawn Eel
                StartCoroutine(StopToPickUpEel());
                spriteRenderer.color = startingColor;
                carrying = false;
            }
        }

        if (canOpenCage && !carrying && !notActive || canPullLever && !carrying && !notActive || canPushButton && !carrying && !notActive)
            PushingAndPulling();
    }
    void FixedUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (!cannotMove && !notActive)
        {
            if (x > 0)
                facingRight = true;
            else if (x < 0)
                facingRight = false;
        }

        if (facingRight && !climbing)
            spriteRenderer.flipX = false;
        else if (!climbing)
            spriteRenderer.flipX = true;

        if (climbing && !cannotMove && !notActive)
            movement = new Vector2(0.0f, y * climbingSpeed);
        else if (!cannotMove && !notActive && carrying)
            movement = new Vector2(x * walkingSpeedWhenCarryingBucket, rb2d.velocity.y);
        else if (!cannotMove && !notActive)
            movement = new Vector2(x * walkingSpeed, rb2d.velocity.y);
        else
            movement = new Vector2(rb2d.velocity.x, rb2d.velocity.y);

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
            if (!Input.GetButton("Jump") && rb2d.velocity.y >= 0.0f)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2);
                jumping = false;
            }
        }

        if (grounded)
            GroundedAnimations();

        if (canClimb && canClimbAfterJumping && !carrying && !cannotMove && !notActive)
            Climbing();
        else
        {
            rb2d.gravityScale = startGravityScale;
            climbing = false;
            canChangeClimbingDirection = false;
        }
    }

    void GroundedAnimations()
    {
        if (x != 0)
        {
            if (carrying)
            {
                //animator.Play(WALKING WITH BUCKET);
            }
            else
            {
                //animator.Play(WALKING);
            }

            if (canPlayStepSoundsAgain)
            {
                StartCoroutine(StepSoundsLoop());
                canPlayStepSoundsAgain = false;
            }
        }
        else
        {
            if (carrying)
            {
                //animator.Play(IDLE WITH BUCKET);
            }
            else
            {
                //animator.Play(IDLE);
            }
        }
    }

    void Climbing()
    {
        if (climbing)
        {
            if (x != 0.0f && canChangeClimbingDirection)
            {
                if (facingRight)
                {
                    transform.position = new Vector3(ladderXPosition + climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                    spriteRenderer.flipX = true;
                }
                else
                {
                    transform.position = new Vector3(ladderXPosition - climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                    spriteRenderer.flipX = false;
                }

                if (y == 0)
                {

                StartCoroutine(StopClimbing());
                }
            }
            if (x == 0.0f)
                canChangeClimbingDirection = true;
        }
        if (y != 0.0f)
        {
            if (!grounded || grounded && y > 0.0f)
            {
                rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
                if (!climbing)
                {
                    rb2d.gravityScale = 0.0f;
                    rb2d.velocity = new Vector2(0.0f, 0.0f);
                    if (transform.position.x <= ladderXPosition)
                    {
                        transform.position = new Vector3(ladderXPosition - climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                        facingRight = true;
                        spriteRenderer.flipX = false;

                    }
                    else
                    {
                        transform.position = new Vector3(ladderXPosition + climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                        facingRight = false;
                        spriteRenderer.flipX = true;
                    }
                    climbing = true;
                }
            }
            else
            {
                rb2d.gravityScale = startGravityScale;
                climbing = false;
                canChangeClimbingDirection = false;
            }
        }

        if (x != 0.0f && canPlayClimbingSoundAgain || y != 0.0f && canPlayClimbingSoundAgain)
        {
            StartCoroutine(ClimbingSoundsLoop());
            canPlayClimbingSoundAgain = false;
        }
    }

    void PushingAndPulling()
    {
        if (canOpenCage)
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
            {
                //animator.Play(CAGE);
                //audioSource.PlayOneShot(CAGE SFX);
                //transform.position = new Vector3(lever.gameObject.transform.position.x, transform.position.y, transform.position.z);
                cage.GetComponent<Cage>().Open();
                StartCoroutine(StopToPullOrPush());
            }
        }
        else if (canPullLever)
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
            {
                //animator.Play(LEVER);
                //audioSource.PlayOneShot(LEVER SFX);
                //transform.position = new Vector3(lever.gameObject.transform.position.x, transform.position.y, transform.position.z);
                lever.GetComponent<ButtonOrLever>().Activate();
                StartCoroutine(StopToPullOrPush());
            }
        }
        else //if you're close to both a lever and a button at the same time you'll always just pull the lever.
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
            {
                //animator.Play(BUTTON);
                //audioSource.PlayOneShot(BUTTON SFX);
                //transform.position = new Vector3(button.gameObject.transform.position.x, transform.position.y, transform.position.z);
                button.GetComponent<ButtonOrLever>().Activate();
                StartCoroutine(StopToPullOrPush());
            }
        }
    }

    void Jump()
    {
        //audioSource.PlayOneShot(jumpSFX);
        //animator.Play(JUMP ANIMATION);
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
        jumping = true;
        rb2d.gravityScale = startGravityScale;
        climbing = false;
        canChangeClimbingDirection = false;
        StartCoroutine(CanClimbAfterJumping());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            ladderXPosition = other.gameObject.transform.position.x;
            canClimb = true;
        }

        if (other.gameObject.tag == "Eel")
            canPickUpEel = true;

        if (other.gameObject.tag == "Lever")
        {
            canPullLever = true;
            lever = other.gameObject;
        }
        if (other.gameObject.tag == "Button")
        {
            canPushButton = true;
            button = other.gameObject;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Human") || other.gameObject.layer == LayerMask.NameToLayer("AboveWater"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Human") && other.gameObject.GetComponent<Human>().charmed) { }
            else
            {
                Debug.Log("am here " + other.gameObject.name);
                scaryObject = other.gameObject;
                audioSource.PlayOneShot(scaredSFX);
                StartCoroutine(WalkAway());
            }
        }

        if (other.gameObject.tag == "Cage")
        {
            canOpenCage = true;
            cage = other.gameObject;
        }

        if (other.gameObject.tag == "Finish")
        {
            monkeyLevelComplete = true;
           
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
            canClimb = false;

        if (other.gameObject.tag == "Eel")
            canPickUpEel = false;

        if (other.gameObject.tag == "Lever")
            canPullLever = false;
        if (other.gameObject.tag == "Button")
            canPushButton = false;

        if (other.gameObject.tag == "Cage")
            canOpenCage = false;

        if (other.gameObject.tag == "Finish")
        {
            monkeyLevelComplete = false;
            
        }
    }

    IEnumerator JumpBufferTimer()
    {
        jumpBuffer = true;
        yield return new WaitForSecondsRealtime(jumpBufferTime);
        jumpBuffer = false;
    }

    IEnumerator CanClimbAfterJumping()
    {
        canClimbAfterJumping = false;
        yield return new WaitForSeconds(canClimbAgainAfterJumpingTime);
        canClimbAfterJumping = true;
    }

    IEnumerator StopClimbing()
    {
        bool holdingRight;
        if (x > 0.0f)
            holdingRight = true;
        else
            holdingRight = false;
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(timeBetweenClimbingSounds / 4);
            if (holdingRight && x > 0.0f && climbing && y == 0 || !holdingRight && x < 0.0f && climbing && y == 0) { }
            else
                yield break;
        }
        rb2d.gravityScale = startGravityScale;
        climbing = false;
        canChangeClimbingDirection = false;
    }

    IEnumerator ClimbingSoundsLoop()
    {
        //audioSource.PlayOneShot(CLIMBING SOUND);
        yield return new WaitForSeconds(timeBetweenClimbingSounds);
        if (climbing && y != 0.0f || climbing && x != 0.0f)
            StartCoroutine(ClimbingSoundsLoop());
        else
            canPlayClimbingSoundAgain = true;
    }

    IEnumerator StepSoundsLoop()
    {
        //audioSource.PlayOneShot(STEP SOUND);
        yield return new WaitForSeconds(timeBetweenStepSounds);
        if (grounded && x != 0)
            StartCoroutine(ClimbingSoundsLoop());
        else
            canPlayStepSoundsAgain = true;
    }

    IEnumerator StopToPullOrPush()
    {
        cannotMove = true;
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(timeStoppedWhenActivating);
        cannotMove = false;
    }

    IEnumerator StopToPickUpEel()
    {
        cannotMove = true;
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(timeStoppedWhenPickingUpEel);
        cannotMove = false;
    }

    IEnumerator WalkAway()
    {
        cannotMove = true;
        if (scaryObject.transform.position.x >= transform.position.x)
        {
            if (carrying)
                rb2d.velocity = new Vector2(-1 * walkingSpeedWhenCarryingBucket, rb2d.velocity.y);
            else
                rb2d.velocity = new Vector2(-1 * walkingSpeed, rb2d.velocity.y);
        }
        else
        {
            if (carrying)
                rb2d.velocity = new Vector2(1 * walkingSpeedWhenCarryingBucket, rb2d.velocity.y);
            else
                rb2d.velocity = new Vector2(1 * walkingSpeed, rb2d.velocity.y);
        }
        Debug.Log("am here " + rb2d.velocity);
        yield return new WaitForSeconds(walkAwayScaredTime);
        cannotMove = false;
        rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
    }
}
