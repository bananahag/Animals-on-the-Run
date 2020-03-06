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
    public bool scared;
    private Rigidbody2D rb;
    BoxCollider2D box;
    private Animator an;
    private SpriteRenderer sr;
    public float maxDistanceRight;
    public float maxDistanceLeft;
    Vector3 distanceRight;
    Vector3 distanceLeft;
    Vector3 moveSpeed;
    Vector3 oldpos;
    Vector3 gizmopos;
    public Vector2 RandomAmountStoppTime;
    private float timer;
    bool turn;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponentInChildren<BoxCollider2D>();
        an = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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
                    turn = true;
                    float stoppTime = Random.Range(RandomAmountStoppTime.x, RandomAmountStoppTime.y);
                    timer += Time.deltaTime;
                    if (timer > stoppTime && !charmed && !scared)
                    {
                        print("moving right");
                        fraction = 0;
                        oldpos = transform.position;
                        movingLeft = !movingLeft;
                        sr.flipX = true;
                        turn = false;
                        timer = 0;
                    }
                    

                }
            }
            else 
            {
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceRight, fraction));
                if (transform.position.x == distanceRight.x)
                {
                    turn = true;
                    float stoppTime = Random.Range(RandomAmountStoppTime.x, RandomAmountStoppTime.y);
                    timer += Time.deltaTime;
                    if (timer > stoppTime && !charmed && !scared)
                    {
                        print("moving left");
                        fraction = 0;
                        oldpos = transform.position;
                        movingLeft = !movingLeft;
                        sr.flipX = false;
                        turn = false;
                        timer = 0;
                    }
                   
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
            an.Play("HumanScared");

        }
        else if (scared)
        {
            moving = false;
            an.Play("HumanIdle");
        }
        else
        {
            box.enabled = true;
            moving = true;

        }
         if (turn && !charmed && !scared)
        {
            an.Play("HumanIdle");
        }
        else if (moving)
        {
            an.Play("HumanWalk");
        }
      



    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-maxDistanceLeft, 0, 0) + gizmopos, new Vector3(maxDistanceRight, 0, 0) + gizmopos);
    }
}
