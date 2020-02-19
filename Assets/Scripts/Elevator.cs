using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
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

    float fraction, timePassed;
    bool goingBack;

    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        fraction = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (fraction < 1.0f && !goingBack)
        {
            timePassed = 0.0f;
            fraction += Time.deltaTime / travelTime;
            if (targetPosition.position.y > startPosition.position.y)
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
            else
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
        }
        else if (fraction > 0.0f && goingBack)
        {
            timePassed = 0.0f;
            fraction -= Time.deltaTime / travelTime;
            if (startPosition.position.y > targetPosition.position.y)
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
            else
                transform.position = Vector2.Lerp(startPosition.position, targetPosition.position, fraction);
        }
        else if (timePassed < stayTime)
            timePassed += Time.deltaTime;
        else
        {
            if (goingBack)
                goingBack = false;
            else
                goingBack = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.SetParent(null);
    }
}
