using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public Transform groundCheckLeft = null, groundCheckRight = null;
    public AudioClip jumpSFX;//+ more sound effects

    public float walkingSpeed = 5.0f;
    public float timeBetweenStepSounds = 0.5f;

    public float climbingHorizontallySpeed = 2.5f, climbingVerticallySpeed = 4.0f;
    public float timeBetweenClimbingSounds = 0.5f;

    public float jumpVelocity = 10.0f;
    public float jumpBufferTime = 0.25f;
    public bool CurrentChar;

    public float timeStoppedWhenPickingUpEel = 1.0f;
    public float timeStoppedWhenActivating = 1.0f;


    AudioSource audioSource;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;

    Vector2 movement;
    float x, y;
    bool cannotMove;

    bool jumping;
    bool jumpBuffer;

    bool grounded;
    bool canPlayStepSoundsAgain;

    bool canPickUpEel;
    bool carrying;

    bool canClimb;
    bool climbing;
    bool canPlayClimbingSoundAgain;

    bool canPullLever, canPushButton;
    GameObject lever, button;

    float ladderXPosition;
    float startGravityScale;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded && !carrying && !cannotMove || jumpBuffer && grounded && !carrying && !cannotMove)
        {
            Jump();
            jumpBuffer = false;
        }

        if (Input.GetButtonDown("Jump") && !grounded && !cannotMove)
        {
            StartCoroutine(JumpBufferTimer());
        }

        if (Input.GetButtonDown("Interact") && !cannotMove && grounded)
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
                spriteRenderer.color = Color.white;
                carrying = false;
            }
        }

        if (canPullLever && !carrying || canPushButton && !carrying)
            PushingAndPulling();
    }
    void FixedUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (climbing && !cannotMove)
            movement = new Vector2(x * climbingHorizontallySpeed, y * climbingVerticallySpeed);
        else if (!cannotMove)
            movement = new Vector2(x * walkingSpeed, rb2d.velocity.y);
        else
            movement = new Vector2(0.0f, rb2d.velocity.y);

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

        if (canClimb && !carrying)
            Climbing();
        else
        {
            rb2d.gravityScale = startGravityScale;
            climbing = false;
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
        if (y != 0.0f)
        {
            if (!grounded || grounded && y > 0.0f)
            {
                rb2d.velocity = new Vector2(0.0f, rb2d.velocity.y);
                if (!climbing)
                {
                    rb2d.gravityScale = 0.0f;
                    rb2d.velocity = new Vector2(0.0f, 0.0f);
                    transform.position = new Vector3(ladderXPosition, transform.position.y, transform.position.z);
                    climbing = true;
                }
            }
            else
            {
                rb2d.gravityScale = startGravityScale;
                climbing = false;
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
        if (canPullLever)
        {
            if (Input.GetButtonDown("Interact") && !cannotMove && grounded)
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
            if (Input.GetButtonDown("Interact") && !cannotMove && grounded)
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
    }

    IEnumerator JumpBufferTimer()
    {
        jumpBuffer = true;
        yield return new WaitForSecondsRealtime(jumpBufferTime);
        jumpBuffer = false;
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
        yield return new WaitForSeconds(timeStoppedWhenActivating);
        cannotMove = false;
    }

    IEnumerator StopToPickUpEel()
    {
        cannotMove = true;
        yield return new WaitForSeconds(timeStoppedWhenPickingUpEel);
        cannotMove = false;
    }

    
}
