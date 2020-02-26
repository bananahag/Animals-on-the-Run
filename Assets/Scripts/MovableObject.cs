using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [Tooltip("If either of these transforms touches an object with the ''Ground'' layer the monkey will be grounded.")]
    public Transform groundCheckLeft = null, groundCheckRight = null;
    Collider2D objectCollider;
    private Transform ColliderTransform;
    public bool carried;
    public bool rightSide; //right = true, left = false
    public GameObject carry;
    public bool grounded;
    public bool canMoveObject;

    void Start()
    {
        ColliderTransform = GetComponent<Transform>();
        objectCollider = ColliderTransform.GetChild(0).GetComponent<Collider2D>();
        canMoveObject = false;
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
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            Physics2D.IgnoreCollision(other.transform.GetComponent<Collider2D>(), objectCollider);
        }

        //canmoveobject

        if (canMoveObject && other.gameObject.CompareTag("MovableObject"))
        {
            Physics2D.IgnoreCollision(other.gameObject.transform.GetComponent<Transform>().GetChild(0).GetComponent<Collider2D>(), objectCollider);
        }
    }

    void OnCollisionExit2D(Collision2D other) //Kan behövas tas bort pga buggar och att GroundCheck finns 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = false;

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
                transform.position = carry.GetComponent<Transform>().position + new Vector3(carry.GetComponent<DogBehaviour>().radius, 0, 0);
            }
            else if(rightSide == false)
            {
                transform.position = carry.GetComponent<Transform>().position - new Vector3(carry.GetComponent<DogBehaviour>().radius, 0, 0);
            }
        }

        if (!grounded)
        {
            Drop();
        }
    }
}
