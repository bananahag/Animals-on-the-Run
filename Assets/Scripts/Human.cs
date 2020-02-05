using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float speed = 3;
    public bool moving;
    public bool charmed;
    private Rigidbody2D rb;
    public BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        charmed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (charmed)
        {
            moving = false;
            rb.isKinematic = true;
            box.enabled = false;
            rb.velocity = Vector3.zero;

        }
        else
        {
            rb.isKinematic = false;
            box.enabled = true;
            moving = true;
        }

        if (moving)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
    }
}
