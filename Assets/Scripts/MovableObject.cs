using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    Collider2D objectCollider;
    private Transform ColliderTransform;
    private bool carried;
    private bool rightSide; //right = true, left = false
    private GameObject carry;
    private bool grounded;

    void Start()
    {
        ColliderTransform = GetComponent<Transform>();
        objectCollider = ColliderTransform.GetChild(0).GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            Physics2D.IgnoreCollision(other.transform.GetComponent<Collider2D>(), objectCollider);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
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

    public void Drop() {
        carry = null;
        carried = false;
    }

    void Update()
    {
        if (carried)
        {
            if(rightSide == true)
            {
                transform.position = carry.GetComponent<Transform>().position + new Vector3(carry.GetComponent<Dog>().radius, 0, 0);
            }
            else if(rightSide == false)
            {
                transform.position = carry.GetComponent<Transform>().position - new Vector3(carry.GetComponent<Dog>().radius, 0, 0);
            }
        }

        if (!grounded)
        {
            Drop();
        }
    }
}
