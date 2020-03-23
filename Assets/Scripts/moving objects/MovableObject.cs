using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Tooltip("Audio source that plays when the box object lands on the ground.")]
    public AudioSource landSource;

    [HideInInspector]
    public bool beingMoved;
    [HideInInspector]
    public bool grounded;
    public Transform groundCheckLeft = null, groundCheckRight = null;
    public int raysNotGrounded = 0;
    float xPos;
    [Tooltip("From what distance the dog can interact with the box.")]
    public float boxDistance;
    [Tooltip("How close the box must be to the ground to be considered grounded.")]
    public float groundCheckDistance;
    [Tooltip("Checks if the box has this layer. It's used by RayCast and should be Ground")]
    public LayerMask boxMask;
    [Tooltip("Checks if ground is below it. It's used by RayCast and should be Ground")]
    public LayerMask groundMask;
    float startMass;
    [Tooltip("Disables collision to player and enables the top collider. Makes it possible to stand on top of the box and still pass through it from the sides.")]
    public bool noCollision = false;
    GameObject topCollider;
    public GameObject botCollider;

    [Tooltip("Checks if there is ground is below the box on it's left side of it.")]
    public float leftGroundCheckoffset = -1.9f;
    [Tooltip("Checks if there is ground is below the box on it's right side of it.")]
    public float rightGroundCheckoffset = 1.9f;

    void OnValidate()
    {
        if (boxDistance == 0)
        {
            boxDistance = 0.4f;
        }
        if (groundCheckDistance == 0)
        {
            groundCheckDistance = 0.4f;
        }
        if (groundMask == 0 && boxMask != 0)
        {
            groundMask = boxMask;
        }

    }

    public bool BoxGrounded()
    {
        if (grounded || raysNotGrounded > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Awake()
    {
        topCollider = gameObject.transform.Find("TopCollider").gameObject;
        botCollider = gameObject.transform.Find("GameObject").gameObject;
        groundCheckLeft = gameObject.transform.Find("Ground Check Left").transform;
        groundCheckRight = gameObject.transform.Find("Ground Check Right").transform;
    }

    void Start()
    {
        if (noCollision)
        {
            topCollider.SetActive(true);
            gameObject.layer = 0;
        }
        else
        {
            //topCollider.SetActive(true);
            topCollider.SetActive(false);
            gameObject.layer = 8;
        }

        xPos = transform.position.x;
        startMass = GetComponent<Rigidbody2D>().mass;
    }

    void GroundCheck()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << 8)
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << 8))
        {
            grounded = true;
        }
        else
        {
            if (!grounded && landSource != null)
            {
                landSource.Play();
            }
            grounded = false;
        }
    }

    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.y, boxDistance);

        if (hitBox.collider != null && !hitBox.collider.CompareTag("TopCollider"))
        {
            Debug.Log(hitBox.collider.gameObject);
            GetComponent<Rigidbody2D>().mass = 100;
        }
        else
        {
            GetComponent<Rigidbody2D>().mass = startMass;
        }

        if (!beingMoved)
        {
            transform.position = new Vector3(xPos, transform.position.y);
        }
        else
        {
            xPos = transform.position.x;
        }
        GroundCheck();
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.y * boxDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + -Vector2.up * transform.localScale.y * groundCheckDistance);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (noCollision)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground") && !other.gameObject.CompareTag("MovableObject"))
            {
                if (other.gameObject.tag == "Eel")
                    Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), other.gameObject.GetComponent<Eel>().collider);
                else
                    Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), other.gameObject.GetComponent<BoxCollider2D>());
            }
        }

    }
}
