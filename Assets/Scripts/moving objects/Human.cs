﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [Tooltip("Time it takes for human to travel from maxdistanceleft to maxdistanceright")]
    public float travelTime = 3;
    private float fraction;
    public bool moving;
    private bool movingLeft = true;
    private bool nearMonkey;

    [Tooltip("Audio source that plays the human gets charmed.")]
    public AudioSource charmedSource;
    [Tooltip("Audio source that plays the human gets scared.")]
    public AudioSource scaredSource;
    [Tooltip("Audio source that plays the audio clips.")]
    public AudioSource stepSource;
    [Tooltip("Audio clips that plays when the dog takes a step on the ground.")]
    public AudioClip[] stepClips;
    [Tooltip("The minimum and the maximum volume of the footstep sounds.")]
    public float minVolume = 0.9f, maxVolume = 1.1f;
    [Tooltip("The minimum and the maximum pitch of the footstep sounds.")]
    public float minPitch = 0.9f, maxPitch = 1.1f;

    MonkeyBehavior monkey;
    public bool charmed;
    public bool scared;
    private Rigidbody2D rb;
    public BoxCollider2D box;
    Animator an;
    public Animation[] anim;
    [HideInInspector]
    public SpriteRenderer sr;
    [Tooltip("Distance human will walk right")]
    public float maxDistanceRight;
    [Tooltip("Distance human will walk left")]
    public float maxDistanceLeft;
    Vector3 distanceRight;
    Vector3 distanceLeft;
    Vector3 moveSpeed;
    Vector3 oldpos;
    Vector3 gizmopos;
    [Tooltip("x amount of minimum seconds, Y amount of maximum seconds -1 before turning around")]
    public Vector2 RandomAmountStoppTime;
    private float timer;
    bool turn;
    public enum HumanState
    {
        Moving = 0,
        Charmed = 1,
        Scared = 2,
    }
    HumanState currentState;
   
    private void Awake()
    {
        box = transform.Find("HumanCollider").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SwitchHumanState(HumanState.Moving);
        an = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        charmed = false;
        scared = false;
        distanceLeft = new Vector3(-maxDistanceLeft, 0, 0) + transform.position;
        distanceRight = new Vector3(maxDistanceRight, 0, 0) + transform.position;
        oldpos = transform.position;
        gizmopos = transform.position;
    }

    void FixedUpdate()
    {
        if (moving)
         {
            if (movingLeft)
            {
                sr.flipX = false;
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceLeft, fraction));
                if (transform.position.x == distanceLeft.x)
                {
                    turn = true;
                    float stoppTime = Random.Range(RandomAmountStoppTime.x, RandomAmountStoppTime.y);
                    timer += Time.deltaTime;
                    if (timer > stoppTime && !charmed && !scared)
                    {
                        fraction = 0;
                        oldpos = transform.position;
                        movingLeft = !movingLeft;
                        
                        turn = false;
                        timer = 0;
                    }
                }
            }
            else 
            {
                sr.flipX = true;
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceRight, fraction));
                if (transform.position.x == distanceRight.x)
                {
                    turn = true;
                    float stoppTime = Random.Range(RandomAmountStoppTime.x, RandomAmountStoppTime.y);
                    timer += Time.deltaTime;
                    if (timer > stoppTime && !charmed && !scared)
                    {
                        fraction = 0;
                        oldpos = transform.position;
                        movingLeft = !movingLeft;
                        
                        turn = false;
                        timer = 0;
                    }
                }
            }
         }
    }

    public void SwitchHumanState(HumanState state)
    {
        currentState = state;
        if (nearMonkey || charmed)
        {
            if (charmed)
            {
                currentState = HumanState.Charmed;

            }
            else if (nearMonkey)
            {
                currentState = HumanState.Scared;
            }
        }
        switch (currentState)
        {
            case HumanState.Moving:
                moving = true;
                scared = false;
                rb.isKinematic = false;
                box.enabled = true;
                break;
            case HumanState.Charmed:
                if(charmedSource != null)
                {
                    charmedSource.Play();
                }
                Debug.Log("Charm noise");
                moving = false;
                scared = false;
                rb.isKinematic = true;
                
                break;
            case HumanState.Scared:
                if(scaredSource != null)
                {
                    scaredSource.Play();
                }
                Debug.Log("Scared noise");
                moving = false;
                rb.isKinematic = true;
                
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (currentState == HumanState.Moving)
        {
            if (turn)
            {
                an.Play("HumanIdle");
            }
            else
            an.Play("HumanWalk");
            //PlayStepSound();
        }
        else if (currentState == HumanState.Scared)
        {
            an.Play("HumanScared");
        }
        else if (currentState == HumanState.Charmed)
        {
            an.Play("HumanCharmed");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-maxDistanceLeft, 0, 0) + gizmopos, new Vector3(maxDistanceRight, 0, 0) + gizmopos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monkey")
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                if(currentState != HumanState.Charmed)
                {
                    sr.flipX = true;
                }
            }
            else
            {
                if (currentState != HumanState.Charmed)
                {
                    sr.flipX = false;
                }
            }
            monkey = collision.gameObject.GetComponent<MonkeyBehavior>();
            nearMonkey = true;
            if(currentState != HumanState.Charmed)
            {
                SwitchHumanState(HumanState.Scared);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monkey")
        {
            nearMonkey = false;
            if(currentState != HumanState.Charmed)
            {
                SwitchHumanState(HumanState.Moving);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Physics2D.IgnoreCollision(box, other.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    public void PlayStepSound()
    {
        if (stepClips.Length > 0)
        {
            int randomSource = Random.Range(0, stepClips.Length);
            stepSource.volume = Random.Range(minVolume, maxVolume);
            stepSource.pitch = Random.Range(minPitch, maxPitch);
            stepSource.PlayOneShot(stepClips[randomSource]);
        }
    }
}
