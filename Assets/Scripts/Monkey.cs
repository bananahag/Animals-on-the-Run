using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;
    public AudioClip jumpSFX, scaredSFX, interactSFX, stepSFX, ladderClimbSFX, topOfLadderSFX, liftEel, dropEel;//+ more sound effects

    public float walkingSpeed = 5.0f;
    public float walkingSpeedWhenCarryingBucket = 3.5f;
    public float stopWhileScaredTime = 0.5f, walkAwayScaredTime = 0.5f;

    public float climbingSpeed = 4.0f;
    public float timeBetweenClimbingSounds = 0.25f;
    public float climbingOffsetFromCenterDistance = 0.25f;
    public float stopClimbingTime = 0.5f;
    public float canClimbAgainAfterJumpingTime = 0.25f;
    public float canChangeClimbingDirectionTime = 0.5f;

    public float jumpVelocity = 10.0f;
    public float jumpSquatTime = 0.125f;
    public float jumpBufferTime = 0.25f;
    public float landingTime = 0.25f;
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
    bool scared;

    bool canPickUpEel;
    bool carrying;

    bool canClimb, canClimbAfterJumping;
    int oneWayLadder; //0 = no one way ladder, 1 = one way ladder facing right, 2 = one way ladder facing left
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
        if (Input.GetButtonDown("Jump") && !grounded && !climbing && !cannotMove && !notActive)
        {
            StartCoroutine(JumpBufferTimer());
        }

        if (Input.GetButtonDown("Jump") && grounded && !carrying && !cannotMove && !notActive || jumpBuffer && grounded && !carrying && !cannotMove && !notActive
            || Input.GetButtonDown("Jump") && climbing && !carrying && !cannotMove && !notActive)
        {
            if (climbing)
                audioSource.PlayOneShot(ladderClimbSFX);
            Jumpsquat();
            jumpBuffer = false;
        }

        if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
        {
            if (!carrying && canPickUpEel)
            {
                animator.Play("Placeholder Monkey Pickup");
                audioSource.PlayOneShot(liftEel);
                StartCoroutine(StopToPickUpEel());
                spriteRenderer.color = Color.cyan;
                carrying = true;
            }
            else if (carrying)
            {
                animator.Play("Placeholder Monkey Pickdown");
                audioSource.PlayOneShot(dropEel);
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

        if (!cannotMove && !notActive && !climbing)
        {
            if (x > 0)
                facingRight = true;
            else if (x < 0)
                facingRight = false;
        }

        if (facingRight)
            spriteRenderer.flipX = false;
        else
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
            if (!grounded)
            {
                audioSource.PlayOneShot(stepSFX);
                animator.Play("Placeholder Monkey Land");
                StartCoroutine(LandingTimer());
            }
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

            if (rb2d.velocity.y < 0.0f)
                jumping = false;
        }

        if (grounded && !cannotMove && rb2d.velocity.y == 0.0f && !jumping && !climbing)
            GroundedAnimations();
        else if (!grounded && !climbing && !cannotMove)
            InAirAnimations();

        if (canClimb && canClimbAfterJumping && !carrying && !cannotMove && !notActive)
            Climbing();
        else
        {
            rb2d.gravityScale = startGravityScale;
            climbing = false;
            canChangeClimbingDirection = false;
            animator.enabled = true;
        }
    }

    void GroundedAnimations()
    {
        if (x != 0)
        {
            if (carrying)
            {
                animator.Play("Placeholder Monkey Walk Bucket");
            }
            else
            {
                animator.Play("Placeholder Monkey Walk");
            }
        }
        else
        {
            if (carrying)
            {
                animator.Play("Placeholder Monkey Idle Bucket");
            }
            else
            {
                animator.Play("Placeholder Monkey Idle");
            }
        }
    }

    void InAirAnimations()
    {
        if (carrying)
        {
            animator.Play("Placeholder Monkey Fall Bucket");
        }
        else
        {
            if (rb2d.velocity.y > 0.0f)
                animator.Play("Placeholder Monkey Jump");
            else if (rb2d.velocity.y < 0.0f)
                animator.Play("Placeholder Monkey Fall");
        }

    }

    void Climbing()
    {
        if (climbing)
        {
            if (y != 0)
                animator.enabled = true;
            else
                animator.enabled = false;
            if (x != 0.0f && canChangeClimbingDirection && oneWayLadder == 0)
            {
                if (x > 0.0f)
                {
                    transform.position = new Vector3(ladderXPosition + climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                    facingRight = false;
                    spriteRenderer.flipX = true;
                }
                else if (x < 0.0f)
                {
                    transform.position = new Vector3(ladderXPosition - climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                    facingRight = true;
                    spriteRenderer.flipX = false;
                }

                if (y == 0)
                {

                    StartCoroutine(StopClimbing());
                }
            }

            if (oneWayLadder != 0 && y == 0)
            {
                if (oneWayLadder == 1 && x > 0.0f)
                    StartCoroutine(StopClimbing());
                else if (oneWayLadder == 2 && x < 0.0f)
                    StartCoroutine(StopClimbing());
            }
            if (x == 0.0f)
                canChangeClimbingDirection = true;

            if (y != 0.0f && canPlayClimbingSoundAgain)
            {
                StartCoroutine(ClimbingSoundsLoop());
                canPlayClimbingSoundAgain = false;
            }
        }
        if (y != 0.0f)
        {
            if (!grounded || grounded && y > 0.0f)
            {
                rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
                if (!climbing)
                {
                    animator.Play("Placeholder Monkey Climb");
                    rb2d.gravityScale = 0.0f;
                    rb2d.velocity = new Vector2(0.0f, 0.0f);
                    if (transform.position.x <= ladderXPosition && oneWayLadder == 0 || oneWayLadder == 2)
                    {
                        transform.position = new Vector3(ladderXPosition - climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                        facingRight = true;

                    }
                    else
                    {
                        transform.position = new Vector3(ladderXPosition + climbingOffsetFromCenterDistance, transform.position.y, transform.position.z);
                        facingRight = false;
                    }
                    climbing = true;
                }
            }
            else
            {
                rb2d.gravityScale = startGravityScale;
                climbing = false;
                canChangeClimbingDirection = false;
                animator.enabled = true;
            }
        }
        else if (climbing && !canChangeClimbingDirection)
            StartCoroutine(CanChangeClimingDirectionTimer());
    }

    void PushingAndPulling()
    {
        if (canOpenCage)
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
            {
                animator.Play("Placeholder Monkey Interact");
                audioSource.PlayOneShot(interactSFX);
                //transform.position = new Vector3(lever.gameObject.transform.position.x, transform.position.y, transform.position.z);
                cage.GetComponent<Cage>().Open();
                StartCoroutine(StopToPullOrPush());
                canOpenCage = false;
            }
        }
        else if (canPullLever)
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
            {
                animator.Play("Placeholder Monkey Interact");
                audioSource.PlayOneShot(interactSFX);
                //transform.position = new Vector3(lever.gameObject.transform.position.x, transform.position.y, transform.position.z);
                lever.GetComponent<ButtonOrLever>().Activate();
                StartCoroutine(StopToPullOrPush());
            }
        }
        else //if you're close to both a lever and a button at the same time you'll always just pull the lever.
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && !notActive && grounded)
            {
                animator.Play("Placeholder Monkey Interact");
                audioSource.PlayOneShot(interactSFX);
                //transform.position = new Vector3(button.gameObject.transform.position.x, transform.position.y, transform.position.z);
                button.GetComponent<ButtonOrLever>().Activate();
                StartCoroutine(StopToPullOrPush());
            }
        }
    }

    void Jumpsquat()
    {
        animator.Play("Placeholder Monkey Jumpsquat");
        StartCoroutine(JumpsquatTimer());
    }

    void Jump()
    {
        audioSource.PlayOneShot(jumpSFX);
        animator.Play("Placeholder Monkey Jump");
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
        jumping = true;
        rb2d.gravityScale = startGravityScale;
        climbing = false;
        canChangeClimbingDirection = false;
        animator.enabled = true;
        StartCoroutine(CanClimbAfterJumping());
    }

    public void PlayStepSound()
    {
        audioSource.PlayOneShot(stepSFX);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            ladderXPosition = other.gameObject.transform.position.x;
            canClimb = true;
            if (other.gameObject.GetComponent<OneWayLadder>() == null)
                oneWayLadder = 0;
            else
            {
                if (other.gameObject.GetComponent<OneWayLadder>().ladderIsFacingRight)
                    oneWayLadder = 1;
                else
                    oneWayLadder = 2;
            }
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

        if (other.gameObject.tag == "Cage" && !other.gameObject.GetComponent<Cage>().opened)
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
        {
            if (y > 0.0f && !carrying && !jumping && rb2d.velocity.y == 4.0f && !Input.GetButtonDown("Jump"))
            {
                audioSource.PlayOneShot(topOfLadderSFX);
            }
            canClimb = false;
            oneWayLadder = 0;
        }

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

    IEnumerator JumpsquatTimer()
    {
        cannotMove = true;
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(jumpSquatTime);
        cannotMove = false;
        Jump();
    }

    IEnumerator JumpBufferTimer()
    {
        jumpBuffer = true;
        yield return new WaitForSecondsRealtime(jumpBufferTime);
        jumpBuffer = false;
    }

    IEnumerator LandingTimer()
    {
        cannotMove = true;
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        yield return new WaitForSeconds(landingTime);
        if (!scared)
            cannotMove = false;
    }

    IEnumerator CanClimbAfterJumping()
    {
        canClimbAfterJumping = false;
        yield return new WaitForSeconds(canClimbAgainAfterJumpingTime);
        canClimbAfterJumping = true;
    }

    IEnumerator CanChangeClimingDirectionTimer()
    {
        yield return new WaitForSeconds(canChangeClimbingDirectionTime);
        if (climbing)
            canChangeClimbingDirection = true;
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
        animator.enabled = true;
    }

    IEnumerator ClimbingSoundsLoop()
    {
        audioSource.PlayOneShot(ladderClimbSFX);
        yield return new WaitForSeconds(timeBetweenClimbingSounds);
        if (climbing && y != 0.0f)
            StartCoroutine(ClimbingSoundsLoop());
        else
            canPlayClimbingSoundAgain = true;
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
        scared = true;
        cannotMove = true;
        Debug.Log("am here " + rb2d.velocity);

        rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
        animator.Play("Placeholder Monkey Scared");

        yield return new WaitForSeconds(stopWhileScaredTime);

        if (scaryObject.transform.position.x >= transform.position.x)
            facingRight = false;
        else
            facingRight = true;

        for (int i = 0; i < 10; i++)
        {
            if (facingRight)
            {
                if (carrying)
                    rb2d.velocity = new Vector2(1 * walkingSpeedWhenCarryingBucket, rb2d.velocity.y);
                else
                    rb2d.velocity = new Vector2(1 * walkingSpeed, rb2d.velocity.y);
            }
            else
            {
                if (carrying)
                    rb2d.velocity = new Vector2(-1 * walkingSpeedWhenCarryingBucket, rb2d.velocity.y);
                else
                    rb2d.velocity = new Vector2(-1 * walkingSpeed, rb2d.velocity.y);
            }

            if (carrying)
            {
                if (grounded)
                    animator.Play("Placeholder Monkey Walk Bucket");
                else
                    animator.Play("Placeholder Monkey Fall Bucket");
            }
            else
            {
                if (grounded)
                    animator.Play("Placeholder Monkey Walk");
                else
                    animator.Play("Placeholder Monkey Fall");
            }
            yield return new WaitForSeconds(walkAwayScaredTime / 10.0f);
        }
        cannotMove = false;
        rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
        scared = false;
    }
}

