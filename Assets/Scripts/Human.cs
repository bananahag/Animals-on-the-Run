using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float travelTime = 3;
    private float fraction;
    public bool moving;
    private bool movingLeft = true;
   
    public bool charmed;
    private Rigidbody2D rb;
    public BoxCollider2D box;
    public float maxDistanceRight;
    public float maxDistanceLeft;
    Vector3 distanceRight;
    Vector3 distanceLeft;
    Vector3 moveSpeed;
    Vector3 oldpos;
    Vector3 gizmopos;

  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponentInChildren<BoxCollider2D>();
        charmed = false;
        distanceLeft = new Vector3(-maxDistanceLeft, 0, 0) + transform.position;
        distanceRight = new Vector3(maxDistanceRight, 0, 0) + transform.position;
        oldpos = transform.position;
        gizmopos = transform.position;
    }

    void FixedUpdate()
    {
        if (moving)
         {
            if (movingLeft)
            {
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceLeft, fraction));
                if (transform.position.x == distanceLeft.x)
                {
                    print("moving right");
                    fraction = 0;
                    oldpos = transform.position;
                    movingLeft = !movingLeft;
                }
            }
            else 
            {
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceRight, fraction));
                if (transform.position.x == distanceRight.x)
                {
                    print("moving left");
                    fraction = 0;
                    oldpos = transform.position;
                    movingLeft = !movingLeft;
                }
            }

         }

       
    }

    void Update()
    {
        if (charmed)
        {
            moving = false;
            box.enabled = false;
            

        }
        else
        {
            box.enabled = true;
            moving = true;
        }

        
       
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-maxDistanceLeft, 0, 0) + gizmopos, new Vector3(maxDistanceRight, 0, 0) + gizmopos);
    }
}
