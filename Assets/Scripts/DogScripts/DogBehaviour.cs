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
    public Vector2 movement;
    [HideInInspector]
    public bool facingRight;
    [HideInInspector]
    public bool carryingBucket;

    public DogGroundedState groundedState = new DogGroundedState();
    public DogInAirState inAirState = new DogInAirState();


    DogState currentState = null;

    public void OnValidate()
    {
        if (groundCheckLeft == null || groundCheckRight == null)
            Debug.LogWarning("At least one player ground check is not assigned!");
    }

    void Awake()
    {
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

        if (facingRight)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        GroundCheck();
        CheckIfInAir();

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

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
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