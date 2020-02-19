using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Tooltip("The sound effect that plays when a player character gets squished.")]
    public AudioClip squishedSFX;

    [Tooltip("The start position of the elevator.")]
    public Transform startPosition;
    [Tooltip("The target position of the elevator.")]
    public Transform targetPosition;
    [Tooltip("The time (in seconds) it takes for the elevator platform to reach its destination.")]
    public float travelTime = 2f;
    [Tooltip("Determines if the platform goes back to its original position after a little while.")]
    public bool returns = true;
    [Tooltip("The time (in seconds) the elevator stops when reaching its destination before going back again (if returns is true).")]
    public float stayTime = 1.0f;
    [Tooltip("Determines if the elevator is activated. If not activated, a lever (or something) will need to be connected to it.")]
    public bool active = true;

    float fraction, timePassed, timePassed2;
    bool goingBack;
    bool goingUp;

    float flattenAmount, widenAmount, timeFlattened = 1.0f;
    bool isFlat;
    bool touchingPlayer;
    GameObject monkey, dog, eel;

    Rigidbody2D rb2d;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        fraction = 0.0f;
        flattenAmount = 1.0f;
        widenAmount = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (fraction < 1.0f && !goingBack)
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
                if (touchingPlayer)
                    transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
                else
                    rb2d.MovePosition(Vector2.Lerp(startPosition.position, targetPosition.position, fraction));
            }
        }
        else if (fraction > 0.0f && goingBack)
        {
            timePassed = 0.0f;
            fraction -= Time.deltaTime / travelTime;
            if (startPosition.position.y > targetPosition.position.y)
            {
                goingUp = true;
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
            }
            else
            {
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
        else
        {
            if (goingBack)
                goingBack = false;
            else
                goingBack = true;
        }

        //if (GetComponent<BoxCollider2D>().bounds.size.x > ) AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHH

            if (touchingPlayer && !goingUp && monkey.transform.position.y > transform.position.y)
            monkey.transform.SetParent(transform);
        else if (monkey != null)
            monkey.transform.SetParent(null);

        if (touchingPlayer && monkey.transform.position.y < transform.position.y && !goingUp && monkey.GetComponent<MonkeyBehavior>().grounded)
            FlattenMonkey();
        if (!touchingPlayer && isFlat)
            UnflattenMonkey();
    }

    void FlattenMonkey()
    {

        if (!isFlat)
            audioSource.PlayOneShot(squishedSFX);
        timePassed2 = 0.0f;
        isFlat = true;
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
        timePassed2 += Time.deltaTime;
        if (timeFlattened < timePassed2)
        {
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
