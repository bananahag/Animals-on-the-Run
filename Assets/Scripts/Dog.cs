﻿using System.Collections;
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
    public float leftInteractPos = -1f;
    public float rightInteractPos = 1f;

    public float yInteractOffsetAbove = 0.9f;
    public float yInteractOffsetBelow = -0.9f;

    private GameObject human;
    private GameObject affectedObject;
    private float interactPosition;


    public float radius;

    Vector2 movement;

    [HideInInspector] public bool dogLevelComplete = false;
    [HideInInspector] public bool lockMovement = false;
    private bool closeToHuman = false;
    private bool charmingHuman = false;
    bool wet = false;
    bool swimming = false;
    bool jumping;
    bool jumpBuffer;
    public bool grounded;
   [HideInInspector] public bool notActive = false;
    bool canMoveObject = false;
    bool movingObject = false;
    bool lockJump = false;
    bool canPlayStepSoundsAgain;
    public bool pushing = false;
    public bool pulling = false;
    private bool positionChecked = false;
    private bool dropBox = false;

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
        //MovementAnimations();

        if (lockMovement)
        {
            rb2d.velocity = new Vector2(0, 0);
        }
        radius = GetComponent<SpriteRenderer>().size.x;

        
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
            if (Input.GetButtonDown("Interact") && canMoveObject && movingObject == false)
            {
                if (affectedObject != null)
                {
                    if (!positionChecked)
                    {
                        if(transform.position.x > affectedObject.transform.position.x)
                        {
                            interactPosition = leftInteractPos;
                        }
                        else
                        {
                            interactPosition = rightInteractPos;
                        }
                        positionChecked = true;
                    }
                    affectedObject.GetComponent<MovableObject>().Pickup(gameObject);
                    movingObject = true;
                    lockJump = true;
                }
            }
            else if(Input.GetButtonDown("Interact") && movingObject)
            {
                dropBox = true;
            }
            if(dropBox)
            {
                affectedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                rb2d.GetComponent<Rigidbody2D>().isKinematic = false;
                affectedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                lockJump = false;
                affectedObject.transform.parent = null;
                positionChecked = false;
                movingObject = false;
                canMoveObject = true;
                dropBox = false;
                affectedObject.GetComponent<MovableObject>().Drop();
            }
            /*if(movingObject && !grounded)
            {
                dropBox = true;
            }*/
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

        if (other.gameObject.CompareTag("MovableObject"))
        {
            affectedObject = other.gameObject;

            Vector3 dir = affectedObject.transform.position - transform.position;
            Debug.Log(dir);
            if (dir.y >= yInteractOffsetAbove || dir.y <= yInteractOffsetBelow)
            {
                canMoveObject = false;
            }
            else
            {
                canMoveObject = true;
            }
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
        if (other.gameObject.tag == "MovableObject")
        {
            canMoveObject = false;
        }
    }
}
