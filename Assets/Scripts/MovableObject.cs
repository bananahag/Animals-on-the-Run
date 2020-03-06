using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [HideInInspector]
    public bool beingMoved;
    public bool grounded;
    float xPos;
    public float boxDistance;
    public float groundCheckDistance;
    public LayerMask boxMask;
    public LayerMask groundMask;
    float startMass;

    public float leftGroundCheckoffset = -1.5f;
    public float rightGroundCheckoffset = 1.5f;

    void OnValidate()
    { 
        if(boxDistance == 0)
        {
            boxDistance = 0.4f;
        }
        if(groundCheckDistance == 0)
        {
            groundCheckDistance = 0.4f;
        }
        if(groundMask == 0 && boxMask != 0)
        {
            groundMask = boxMask;
        }
    }

    public bool BoxGrounded()
    {
        if (grounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Start()
    {
        xPos = transform.position.x;
        startMass = GetComponent<Rigidbody2D>().mass;
    }

    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitBox = Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.y, boxDistance, boxMask);

        if (hitBox.collider != null)
        {
            if (hitBox.collider.CompareTag("MovableObject"))
            {
                GetComponent<Rigidbody2D>().mass = 100;
            }
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

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, -Vector2.up * transform.localScale.y, groundCheckDistance, groundMask);

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitGroundLeft = Physics2D.Raycast(transform.position, -Vector2.up * leftGroundCheckoffset, groundCheckDistance, groundMask);

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitGroundRight = Physics2D.Raycast(transform.position, -Vector2.up * rightGroundCheckoffset, groundCheckDistance, groundMask);

        if (hitGround.collider != null || hitGroundLeft.collider != null || hitGroundRight.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.y * boxDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + -Vector2.up * transform.localScale.y * groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x + rightGroundCheckoffset, transform.position.y) + -Vector2.up * groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x + leftGroundCheckoffset, transform.position.y) + -Vector2.up * groundCheckDistance);

    }

}
