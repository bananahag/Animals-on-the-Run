using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Tooltip("The sound effect that plays when a player character gets squished.")]
    public AudioClip squishedSFX;
    [Tooltip("The sound effect that plays when a player grows agter being squished.")]
    public AudioClip growSFX;

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

    float flattenAmount, widenAmount, timeFlattened = 1.0f;
    bool isFlat, canPlayFlattenSound;
    bool touchingPlayer;
    GameObject monkey, dog, eel;

    Rigidbody2D rb2d;
    AudioSource audioSource;
    BoxCollider2D bc2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        fraction = 0.0f;
        flattenAmount = 1.0f;
        canPlayFlattenSound = true;
        widenAmount = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (fraction < 1.0f && !goingBack && active)
        {
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
            goingBack = true;
            timePassed = 0.0f;
            fraction -= Time.deltaTime / travelTime;
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
            timePassed += Time.deltaTime;
            goingUp = false;
        }
        else if (returns)
        {
            if (goingBack)
                goingBack = false;
            else
                goingBack = true;
            if (looping)
                returns = true;
            else
                returns = false;
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
        if (!touchingPlayer && isFlat)
            UnflattenMonkey();
    }

    void FlattenMonkey()
    {

        if (canPlayFlattenSound)
            audioSource.PlayOneShot(squishedSFX);
        timePassed2 = 0.0f;
        isFlat = true;
        canPlayFlattenSound = false;
        flattenAmount -= 0.1f / travelTime;
        widenAmount += 0.1f / travelTime;
        if (flattenAmount < 0.2f)
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
        canPlayFlattenSound = true;
        timePassed2 += Time.deltaTime;
        if (timeFlattened < timePassed2)
        {
            if (flattenAmount <= 0.2f)
            {
                audioSource.PlayOneShot(growSFX);
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
