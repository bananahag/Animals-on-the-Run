using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyBehavior : MonoBehaviour
{
    [Tooltip("If either of these transforms touches an object with the ''Ground'' layer the monkey will be grounded.")]
    public Transform groundCheckLeft = null, groundCheckRight = null;
    [HideInInspector]
    public bool grounded, jumping, jumpBuffer;
    [HideInInspector]
    public float landingVelocity, startGravityScale;

    [HideInInspector]
    public Animator animator = null;
    [HideInInspector]
    public SpriteRenderer spriteRenderer = null;
    [HideInInspector]
    public Rigidbody2D rb2d = null;

    [HideInInspector]
    public int oneWayLadder;//0 = no one way ladder, 1 = one way ladder facing right, 2 = one way ladder facing left
    [HideInInspector]
    public float x, y, ladderXPosition;
    [HideInInspector]
    public bool facingRight, carryingBucket, canClimb, canPickUpEel, canPullLever, canOpenCage, scaredCheck, runAwayScared, monkeyLevelComplete;
    [HideInInspector]
    public Vector2 movement;
    [HideInInspector]
    public GameObject eel = null, lever = null, cage = null, scaryObject = null;

    [HideInInspector]
    public bool active;

    public MonkeyGrounded groundedState = new MonkeyGrounded();
    public MonkeyJumpsquat jumpsquatState = new MonkeyJumpsquat();
    public MonkeyInAir inAirState = new MonkeyInAir();
    public MonkeyLanding landingState = new MonkeyLanding();
    public MonkeyClimbing climbingState = new MonkeyClimbing();
    public MonkeyPickingUp pickingUpState = new MonkeyPickingUp();
    public MonkeyPuttingDown puttingDownState = new MonkeyPuttingDown();
    public MonkeyInteract interactState = new MonkeyInteract();
    public MonkeyScared scaredState = new MonkeyScared();

    MonkeyState currentState = null;

    public void OnValidate()
    {
        groundedState.OnValidate(this);
        jumpsquatState.OnValidate(this);
        inAirState.OnValidate(this);
        landingState.OnValidate(this);
        climbingState.OnValidate(this);
        pickingUpState.OnValidate(this);
        puttingDownState.OnValidate(this);
        interactState.OnValidate(this);
        scaredState.OnValidate(this);

        if (groundCheckLeft == null || groundCheckRight == null)
            Debug.LogWarning("At least one player ground check is not assigned!");
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        startGravityScale = rb2d.gravityScale;

        active = true;

        currentState = groundedState;
        currentState.Enter();
    }

    void Update()
    {
        currentState.Update();
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (active)
                active = false;
            else
                active = true;
        }
    }

    void FixedUpdate()
    {
        if (active && !scaredCheck)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            x = 0.0f;
            y = 0.0f;
        }

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

    public void DropEel()
    {
        if (eel != null && eel.GetComponent<Eel>() != null)
            eel.GetComponent<Eel>().MonkeyInteraction(false);
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
        {
            eel = other.gameObject;
            canPickUpEel = true;
        }

        if (other.gameObject.tag == "Lever")
        {
            canPullLever = true;
            lever = other.gameObject;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Human") || other.gameObject.layer == LayerMask.NameToLayer("AboveWater"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("AboveWater"))
                runAwayScared = true;
            else
                runAwayScared = false;
            if (other.gameObject.layer == LayerMask.NameToLayer("Human") && other.gameObject.GetComponent<Human>().charmed) { }
            else
            {
                scaryObject = other.gameObject;
                scaredCheck = false;
                ChangeState(scaredState);
            }
        }

        if (other.gameObject.tag == "Cage" && !other.gameObject.GetComponent<Cage>().opened)
        {
            canOpenCage = true;
            cage = other.gameObject;
        }

        if (other.gameObject.tag == "Finish")
            monkeyLevelComplete = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
            canClimb = false;

        if (other.gameObject.tag == "Eel")
            canPickUpEel = false;

        if (other.gameObject.tag == "Lever")
            canPullLever = false;

        if (other.gameObject.tag == "Cage")
            canOpenCage = false;

        if (other.gameObject.tag == "Finish")
            monkeyLevelComplete = false;
    }

    public void ChangeState(MonkeyState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }
}
