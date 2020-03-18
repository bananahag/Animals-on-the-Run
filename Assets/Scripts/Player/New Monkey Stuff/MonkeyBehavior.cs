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
    public bool facingRight, carryingBucket, canClimb, canPickUpEel, canPullLever, canOpenCage, touchingThorns, scaredCheck, runAwayScared, monkeyLevelComplete, runToRight, runRightCheck;
    [HideInInspector]
    public Vector2 movement;
    [HideInInspector]
    public GameObject eel = null, lever = null, cage = null, scaryObject = null;

    [HideInInspector]
    public bool active;
    
    
    [HideInInspector]
    public List<GameObject> humansHit;
    Vector3 distance;

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
    private void Start()
    {
        humansHit = new List<GameObject>();
        distance = new Vector3(scaredState.reactToHumanDistance * 0.5f, 0, 0);
    }

    void Update()
    {
        currentState.Update();
        

        
        if(touchingThorns && currentState != groundedState && touchingThorns && currentState != jumpsquatState)
        {
            runAwayScared = true;
            scaredCheck = false;
            ChangeState(scaredState);
            touchingThorns = false;
        }
    }

    void FixedUpdate()
    {
        humansHit.Clear();
        RaycastHit2D[] humans = Physics2D.RaycastAll(transform.position - distance + Vector3.up, Vector3.right, scaredState.reactToHumanDistance, 1 << 9);
        foreach (RaycastHit2D human in humans)
        {
            if (!human.collider.gameObject.GetComponentInParent<Human>().charmed && human.collider.gameObject.tag != "Button")
            {
                humansHit.Add(human.collider.gameObject);
                if (currentState != scaredState && grounded)
                {
                ChangeState(scaredState);
                }
               
            }

        }

        if (active && !scaredCheck)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }
        else if (runToRight)
        {
            x = 1.0f;
            y = 0.0f;
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
        if (eel != null && eel.GetComponent<Eel>() != null && carryingBucket)
        {
            if (facingRight)
                facingRight = false;
            else
                facingRight = true;
            ChangeState(puttingDownState);
        }
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
            if (other.gameObject.GetComponent<Eel>() != null)
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

          
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Thorns"))
        {
            touchingThorns = true;
            scaryObject = other.gameObject;
        }

        if (other.gameObject.tag == "Cage" && !other.gameObject.GetComponent<Cage>().opened)
        {
            canOpenCage = true;
            cage = other.gameObject;
        }

        if (other.gameObject.tag == "Finish")
            monkeyLevelComplete = true;

        if (other.gameObject.tag == "WalkRight")
            runRightCheck = true;
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

        if (other.gameObject.layer == LayerMask.NameToLayer("Thorns"))
            touchingThorns = false;

        if (other.gameObject.tag == "Finish")
            monkeyLevelComplete = false;

        if (other.gameObject.tag == "WalkRight")
            runRightCheck = false;
    }

    public void ChangeState(MonkeyState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position - distance + Vector3.up, transform.position + distance + Vector3.up);
    }
}
