using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehaviour : MonoBehaviour
{
    [Tooltip("If either of these transforms touches an object with the ''Ground'' layer the monkey will be grounded.")]
    public Transform groundCheckLeft = null, groundCheckRight = null;
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool jumping;
    [HideInInspector]
    public float landingVelocity;
    [HideInInspector]
    public float startGravityScale;


    [HideInInspector]
    public AudioSource audioSource = null;
    [HideInInspector]
    public Animator animator = null;
    [HideInInspector]
    public SpriteRenderer spriteRenderer = null;
    [HideInInspector]
    public Rigidbody2D rb2d = null;

    [HideInInspector]
    public float x;
    [HideInInspector]
    public float radius;
    [HideInInspector]
    public float timePassed2 = 0.0f;
    [Tooltip("How long the dog fur stays wet. Time in seconds. Must be a positive value.")]
    public float wetDuration = 8.0f;
    [HideInInspector]
    public float direction = 1;

    [HideInInspector]
    public Vector2 movement;
    [HideInInspector]
    public bool facingRight;
    [HideInInspector]
    public bool active;
    [HideInInspector]
    public bool jumpBuffer;
    [HideInInspector]
    public bool movingObject = false;
    [HideInInspector]
    public bool canMoveObject;
    [HideInInspector]
    public bool closeToHuman = false;
    [HideInInspector]
    public bool charmingHuman = false;
    [HideInInspector]
    public bool wet = false;
    [HideInInspector]
    public bool swimming = false;
    [HideInInspector]
    public bool pushSideIsLeft;
    [HideInInspector]
    public bool landing;
    [HideInInspector]
    public bool closeToRope, pullingRope;

    public LayerMask humanLayerMask;
    public Vector3 charmDistanceVector;

    [HideInInspector]
    public GameObject affectedObject;
    [HideInInspector]
    public GameObject human, rope;
    [HideInInspector]
    public bool levelCompleted = false, runToRight, runRightCheck;

    public DogGroundedState groundedState = new DogGroundedState();
    public DogInAirState inAirState = new DogInAirState();
    public DogPushingState pushingState = new DogPushingState();
    public DogCharmingState charmingState = new DogCharmingState();
    public DogJumpsquatState jumpsquatState = new DogJumpsquatState();
    public DogSwimmingState swimmingState = new DogSwimmingState();

    DogState currentState = null;

    public void OnValidate()
    {
        groundedState.OnValidate(this);
        inAirState.OnValidate(this);
        pushingState.OnValidate(this);
        charmingState.OnValidate(this);
        jumpsquatState.OnValidate(this);
        swimmingState.OnValidate(this);

        if (groundCheckLeft == null || groundCheckRight == null)
        {
            Debug.LogWarning("At least one player ground check is not assigned!");
        }
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        active = true;
        currentState = groundedState;
        currentState.Enter();
        startGravityScale = rb2d.gravityScale;
        radius = GetComponent<SpriteRenderer>().size.x;
        humanLayerMask = 1 << 9;
        charmDistanceVector = new Vector3(charmingState.charmDistance * 0.5f, 0, 0);
    }

    void Start()
    {
    }

    public IEnumerator MoveObjectCoolDown()
    {
        canMoveObject = false;
        yield return new WaitForSecondsRealtime(0.25f);
        canMoveObject = true;
    }

    public IEnumerator PlayLandingAnimation()
    {
        landing = true;
        yield return new WaitForSecondsRealtime(0.5f);
        landing = false;
    }

    void Update()
    {
        currentState.Update();
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * direction * transform.localScale.x * groundedState.boxInteractDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position -charmDistanceVector, transform.position + charmDistanceVector);

    }

    void FixedUpdate()
    {
        if (active)
        {
            x = Input.GetAxisRaw("Horizontal");
        }
        else if (runToRight)
            x = 1.0f;
        else
        {
            x = 0.0f;
        }

        if (facingRight)
        {
            spriteRenderer.flipX = false;
            direction = 1;
        }
        else
        {
            spriteRenderer.flipX = true;
            direction = -1;
        }

        GroundCheck();

        currentState.FixedUpdate();

        rb2d.velocity = movement;
    }
    public void PlayStepSound()
    {
        groundedState.PlayStepSound();
    }

    void GroundCheck()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << 8)
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << 8))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    public void WetTimer()
    {
        wet = true;
        timePassed2 += Time.deltaTime;
        if (wetDuration < timePassed2)
        {
            wet = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            swimming = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Rope"))
        {
            closeToRope = true;
            rope = other.gameObject;
        }
        if (other.gameObject.tag == "WalkRight")
            runRightCheck = true;
        currentState.OnTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Rope"))
            closeToRope = false;
        if (other.gameObject.tag == "WalkRight")
            runRightCheck = false;
        currentState.OnTriggerExit2D(other);
    }

    public void ChangeState(DogState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }

    public void CheckIfInAir()
    {
        if(grounded != true)
        {
            ChangeState(inAirState);
        }
    }
}