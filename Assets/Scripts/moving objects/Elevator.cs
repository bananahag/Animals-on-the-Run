using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Tooltip("The audio source that plays when a player character gets squished.")]
    public AudioSource squishedSource;
    [Tooltip("What is this audio source? I don't know. Don't ask Albin about this because he won't be able to help you.")]
    public AudioSource mysterySource;
    [Tooltip("The audio source that plays when a player grows after being squished.")]
    public AudioSource growSource;
    [Tooltip("The audio source that plays and loops when the elevator moves.")]
    public AudioSource elevatorMoveSource;
    [Tooltip("The audio source that plays when the elevator starts moving.")]
    public AudioSource startSource;
    [Tooltip("The audio source that plays when the elevator stops moving.")]
    public AudioSource stopSource;

    [Tooltip("The start position of the elevator.")]
    public Transform startPosition;
    [Tooltip("The target position of the elevator.")]
    public Transform targetPosition;
    [Tooltip("The time (in seconds) it takes for the elevator platform to reach its destination.")]
    public float travelTime = 2f;
    [Tooltip("Determines if the platform goes back to its original position after a little while.")]
    public bool returns = true;
    [Tooltip("Determines if the platform movement keeps going back and forth.")]
    public bool looping = true;
    [Tooltip("The time (in seconds) the elevator stops when reaching its destination before going back again (if returns is true).")]
    public float stayTime = 1.0f;
    [Tooltip("Determines if the elevator is activated. If not activated, a lever (or something) will need to be connected to it.")]
    public bool active = true;

    float fraction, timePassed, timePassed2;
    bool goingBack;
    bool goingUp;
    bool canPlayLoopSound;

    float flattenAmount, widenAmount, timeFlattened = 1.0f;
    bool isFlat;
    bool touchingPlayer;
    GameObject monkey, dog, eel;
    GameObject lever = null;

    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Start()
    {
        canPlayLoopSound = true;
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        fraction = 0.0f;
        flattenAmount = 1.0f;
        widenAmount = 1.0f;
        elevatorMoveSource.loop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fraction < 1.0f && !goingBack && active)
        {
            if (elevatorMoveSource != null && canPlayLoopSound)
            {
                elevatorMoveSource.Play();
                if (startSource != null)
                    startSource.Play();
                canPlayLoopSound = false;
            }
            timePassed = 0.0f;
            fraction += Time.deltaTime / travelTime;
            if (targetPosition.position.y > startPosition.position.y)
            {
                goingUp = true;
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
            }
            else
            {
                goingUp = false;
                if (touchingPlayer)
                    transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
                else
                    rb2d.MovePosition(Vector2.Lerp(startPosition.position, targetPosition.position, fraction));
            }
        }
        else if (fraction > 0.0f && goingBack || !active)
        {
            if (elevatorMoveSource != null && canPlayLoopSound && fraction > 0.0f)
            {
                elevatorMoveSource.Play();
                if (startSource != null)
                    startSource.Play();
                canPlayLoopSound = false;
            }
            else if (elevatorMoveSource != null && fraction <= 0.0f)
            {
                elevatorMoveSource.Stop();
                if (!canPlayLoopSound && stopSource != null)
                    stopSource.Play();
                canPlayLoopSound = true;
            }
            goingBack = true;
            timePassed = 0.0f;
            fraction -= Time.deltaTime / travelTime;
            if (fraction <= 0.0f)
                fraction = 0.0f;
            if (startPosition.position.y > targetPosition.position.y)
            {
                if (!active)
                    goingBack = true;
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
            }
            else
            {
                goingUp = false;
                if (touchingPlayer)
                    transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
                else
                    rb2d.MovePosition(Vector2.Lerp(startPosition.position, targetPosition.position, fraction));
            }
        }
        else if (timePassed < stayTime)
        {
            if (elevatorMoveSource != null)
                elevatorMoveSource.Stop();
            if (!canPlayLoopSound && stopSource != null)
                stopSource.Play();
            canPlayLoopSound = true;
            timePassed += Time.deltaTime;
            goingUp = false;
        }
        else if (returns)
        {
            if (!canPlayLoopSound && stopSource != null)
                stopSource.Play();
            canPlayLoopSound = true;
            if (elevatorMoveSource != null)
                elevatorMoveSource.Stop();
            if (goingBack)
                goingBack = false;
            else
                goingBack = true;

            if (!looping)
            {
                if (lever != null)
                    lever.GetComponent<ButtonOrLever>().Activate();
            }
        }
        else
        {
            if (!canPlayLoopSound && stopSource != null)
                stopSource.Play();
            canPlayLoopSound = true;
            if (elevatorMoveSource != null)
                elevatorMoveSource.Stop();
        }

        if (touchingPlayer && !goingUp && monkey.transform.position.y > transform.position.y)
            monkey.transform.SetParent(transform);
        else if (monkey != null)
            monkey.transform.SetParent(null);

        if (monkey != null && transform.position.x <= monkey.transform.position.x && transform.position.x + (bc2d.bounds.size.x / 2.0f) >= monkey.transform.position.x
            || monkey != null && transform.position.x > monkey.transform.position.x && transform.position.x - (bc2d.bounds.size.x / 2.0f) < monkey.transform.position.x)
        {
            if (touchingPlayer && monkey.transform.position.y < transform.position.y && !goingUp && monkey.GetComponent<MonkeyBehavior>().grounded)
                FlattenMonkey();
        }
        if (!touchingPlayer && isFlat || monkey != null && monkey.transform.position.y > transform.position.y && isFlat)
            UnflattenMonkey();
    }

    public void Activate(bool activated, GameObject other)
    {
        lever = other;
        if (activated)
        {
            active = true;
            goingBack = false;
        }
        else
        {
            active = false;
        }
    }

    void FlattenMonkey()
    {
        if (!isFlat)
        {
            int whatDoesThisDo = Random.Range(1, 11);
            if (whatDoesThisDo == 10 && mysterySource != null)
                mysterySource.Play();
            if (squishedSource != null)
                squishedSource.Play();
        }
        timePassed2 = 0.0f;
        isFlat = true;
        flattenAmount -= 0.1f / travelTime;
        widenAmount += 0.1f / travelTime;
        if (flattenAmount < 0.2f && active)
            monkey.GetComponent<MonkeyBehavior>().active = false;
        if (flattenAmount <= 0.05f)
        {
            flattenAmount = 0.05f;
            widenAmount = 1.95f;
        }
        monkey.transform.localScale = new Vector2(widenAmount, flattenAmount);
    }

    void UnflattenMonkey()//Unflatten is probably a word
    {
        timePassed2 += Time.deltaTime;
        if (timeFlattened < timePassed2)
        {
            if (flattenAmount <= 0.2f)
            {
                if (growSource != null)
                    growSource.Play();
                flattenAmount = 0.2f;
            }
            flattenAmount += 0.2f;
            widenAmount -= 0.2f;
            if (flattenAmount >= 1.0f)
            {
                flattenAmount = 1.0f;
                widenAmount = 1.0f;
                isFlat = false;
            }
            if (touchingPlayer)
                monkey.transform.localScale = new Vector2(widenAmount / transform.localScale.x, flattenAmount / transform.localScale.y);
            else
                monkey.transform.localScale = new Vector2(widenAmount, flattenAmount);
        }
        else
        {
            flattenAmount += 0.05f;
            widenAmount -= 0.05f;
            if (flattenAmount >= 0.2f)
            {
                flattenAmount = 0.2f;
                widenAmount = 1.8f;
                monkey.GetComponent<MonkeyBehavior>().active = true;
            }
            if (touchingPlayer)
                monkey.transform.localScale = new Vector2(widenAmount / transform.localScale.x, flattenAmount / transform.localScale.y);
            else
                monkey.transform.localScale = new Vector2(widenAmount, flattenAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monkey")
        {
            touchingPlayer = true;
            monkey = other.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monkey")
            touchingPlayer = false;
    }
}
