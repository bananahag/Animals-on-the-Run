using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyBehavior : MonoBehaviour
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
    public float x, y;
    [HideInInspector]
    public Vector2 movement;
    [HideInInspector]
    public bool facingRight;
    [HideInInspector]
    public bool carryingBucket;

    public MonkeyGrounded groundedState = new MonkeyGrounded();
    public MonkeyJumpsquat jumpsquatState = new MonkeyJumpsquat();
    public MonkeyInAir inAirState = new MonkeyInAir();
    public MonkeyLanding landingState = new MonkeyLanding();
    public MonkeyClimbing climbingState = new MonkeyClimbing();

    MonkeyState currentState = null;

    public void OnValidate()
    {
        groundedState.OnValidate(this);
        jumpsquatState.OnValidate(this);
        inAirState.OnValidate(this);
        landingState.OnValidate(this);
        climbingState.OnValidate(this);

        if (groundCheckLeft == null || groundCheckRight == null)
            Debug.LogWarning("At least one player ground check is not assigned!");
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        startGravityScale = rb2d.gravityScale;

        currentState = groundedState;
        currentState.Enter();
    }

    void Update()
    {
        currentState.Update();
    }

    void FixedUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (facingRight)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        GroundCheck();

        currentState.FixedUpdate();

        rb2d.velocity = movement;
    }

    void GroundCheck()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
            grounded = true;
        else
            grounded = false;
    }

    public void PlayStepSound()
    {
        groundedState.PlayStepSound();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentState.OnTriggerExit2D(other);
    }

    public void ChangeState(MonkeyState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }
}
