using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    Collider2D objectCollider;
    private Transform ColliderTransform;
    private bool carried;
    private bool side; //right = true, left = false
    private GameObject carry;

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
    }

    public bool isCarried()
    {
        return carried;
    }

    public void pickup(GameObject carrier)
    {
        carried = true;
        carry = carrier;
        if(this.GetComponent<Transform>().position.x - carrier.GetComponent<Transform>().position.x < 0)
        {
            side = true;
        } else
        {
            side = false;
        }

    }

    public void drop() {
        carry = null;
        carried = false;
    }

    void Update()
    {
        if (carried)
        {
            transform.position = carry.GetComponent<Transform>().position + new Vector3(carry.GetComponent<Dog>().radius, 0, 0);
        }
    }
}
