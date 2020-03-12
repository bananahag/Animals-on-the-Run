using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [Tooltip("Time it takes for human to travel from maxdistanceleft to maxdistanceright")]
    public float travelTime = 3;
    private float fraction;
    public bool moving;
    private bool movingLeft = true;
   
    public bool charmed;
    public bool scared;
    private Rigidbody2D rb;
    public BoxCollider2D box;
    private Animator an;
    private SpriteRenderer sr;
    [Tooltip("Distance human will walk right")]
    public float maxDistanceRight;
    [Tooltip("Distance human will walk left")]
    public float maxDistanceLeft;
    Vector3 distanceRight;
    Vector3 distanceLeft;
    Vector3 moveSpeed;
    Vector3 oldpos;
    Vector3 gizmopos;
    [Tooltip("x amount of minimum seconds, Y amount of maximum seconds -1 before turning around")]
    public Vector2 RandomAmountStoppTime;
    private float timer;
    bool turn;
   


    private void Awake()
    {
        box = transform.Find("HumanCollider").GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        an = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        charmed = false;
        scared = false;
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
                sr.flipX = false;
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceLeft, fraction));
                if (transform.position.x == distanceLeft.x)
                {
                    turn = true;
                    float stoppTime = Random.Range(RandomAmountStoppTime.x, RandomAmountStoppTime.y);
                    timer += Time.deltaTime;
                    if (timer > stoppTime && !charmed && !scared)
                    {
                        
                        fraction = 0;
                        oldpos = transform.position;
                        movingLeft = !movingLeft;
                        
                        turn = false;
                        timer = 0;
                    }
                    

                }
            }
            else 
            {
                sr.flipX = true;
                fraction += Time.deltaTime / travelTime;
                rb.MovePosition(Vector2.Lerp(oldpos, distanceRight, fraction));
                if (transform.position.x == distanceRight.x)
                {
                    turn = true;
                    float stoppTime = Random.Range(RandomAmountStoppTime.x, RandomAmountStoppTime.y);
                    timer += Time.deltaTime;
                    if (timer > stoppTime && !charmed && !scared)
                    {
                        
                        fraction = 0;
                        oldpos = transform.position;
                        movingLeft = !movingLeft;
                        
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
            rb.isKinematic = true;
            box.enabled = false;
            moving = false;
            an.Play("HumanScared");
            
            
        }
        else if (scared)
        {
            rb.isKinematic = true;
            box.enabled = false;
            moving = false;
            an.Play("HumanScared");
        }
        else if (!charmed || !scared)
        {
            rb.isKinematic = false; 
            moving = true;
            
        }
        
         if (turn && (!charmed && !scared))
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monkey")
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
            scared = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monkey")
        {
            scared = false;
            
        }
    }
}
