using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingScale : MonoBehaviour
{
    
    public GameObject OtherScale;
    public GameObject AnchorPointTop;
    public GameObject AnchorPoint;
    private Vector3 otherScaleVelocityRB;
    public float goingUp;
    public float goingDown;
    public int amountofboxes = 0;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        otherScaleVelocityRB = OtherScale.gameObject.GetComponent<Rigidbody2D>().velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (amountofboxes == 0)
        {
            transform.position = AnchorPoint.transform.position;
        }
        else
        {

        }
        
    }
}
