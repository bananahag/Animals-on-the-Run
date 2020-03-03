using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Tooltip("If either of these transforms touches an object with the ''Ground'' layer the monkey will be grounded.")]
    public Transform groundCheckLeft = null, groundCheckRight = null;
    [HideInInspector]
    public Collider2D objectCollider;
    [HideInInspector]
    public Collider2D topCollider;
    [HideInInspector]
    private Transform ColliderTransform;
    [HideInInspector]
    public bool carried;
    [HideInInspector]
    public bool rightSide; //right = true, left = false
    [HideInInspector]
    public GameObject carry;
    [HideInInspector]
    public bool grounded;
    //[HideInInspector]
    public bool canMoveObject;

    public bool underBox;
    public bool hitRightSide;
    public bool hitLeftSide;

    public bool collideWithPlayer;
    public float pickupOffset = 0.05f;

    public Rigidbody2D boxRb;

    void Start()
    {
        ColliderTransform = GetComponent<Transform>();
        objectCollider = ColliderTransform.GetChild(0).GetComponent<Collider2D>();
        topCollider = ColliderTransform.GetChild(0).GetComponent<Collider2D>();
        canMoveObject = false;
        boxRb = GetComponent<Rigidbody2D>();
    }
    void GroundCheck()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground") && other.gameObject.CompareTag("MovableObject"))
        {
            if (!collideWithPlayer)
            {
                //Physics2D.IgnoreCollision(other.transform.GetComponent<Collider2D>(), objectCollider);
            }
        }
        if (canMoveObject && other.gameObject.CompareTag("MovableObject"))
        {


            //Physics2D.IgnoreCollision(other.gameObject.transform.GetComponent<Transform>().GetChild(0).GetComponent<Collider2D>(), objectCollider);
        }
        
    }

    public bool IsCarried()
    {
        return carried;
    }

    public void Pickup(GameObject carrier)
    {
        if (canMoveObject)
        {
            carried = true;
            carry = carrier;
            if(this.GetComponent<Transform>().position.x  > carry.transform.position.x)
            {
                rightSide = true;
            } else
            {
                rightSide = false;
            }
            boxRb.velocity = new Vector2(boxRb.velocity.x, 0);

        }
    }

    public void Drop() {
        carry = null;
        carried = false;
    }

    void Update()
    {
        GroundCheck();
        if (carried)
        {
            if(rightSide == true)
            {
                transform.position = carry.GetComponent<Transform>().position + new Vector3(carry.GetComponent<DogBehaviour>().radius + pickupOffset, 0, 0);
            }
            else if(rightSide == false)
            {
                transform.position = carry.GetComponent<Transform>().position - new Vector3(carry.GetComponent<DogBehaviour>().radius + pickupOffset, 0, 0);
            }
        }
        if (!grounded)
        {
            canMoveObject = false;
            if (carried)
            {
                Drop();
            }
        }
    }
}
