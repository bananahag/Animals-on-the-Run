using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    Collider2D objectCollider;
    private Transform ColliderTransform;

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
}
