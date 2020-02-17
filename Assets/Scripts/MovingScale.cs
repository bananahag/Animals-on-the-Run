using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingScale : MonoBehaviour
{
    
    public GameObject OtherScale;
    public List<GameObject> leftAnchorPoints;
    public List<GameObject> rightAnchorPoints;
    public float speed = 1.0f;
    public int amountofboxes = 0;
    private float startTime;
   
    
    bool move;
    void Start()
    {
        print(leftAnchorPoints.Count);
        float startTime = Time.time;
        
        
       
 
     

    }

    // Update is called once per frame
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;

        
        MoveScale(distCovered);
        
    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.tag == "MoveableObject")
        {
            print(collision.tag);
            amountofboxes++;
           // collision.gameObject.transform.SetParent(transform);
           
        }

    }

    void MoveScale(float distCovered)
    {
     
        if (amountofboxes == 1)
        {
            float fracjourney = distCovered / Vector3.Distance(leftAnchorPoints[0].transform.position, leftAnchorPoints[1].transform.position);
            transform.position = Vector3.Lerp(leftAnchorPoints[0].transform.position, leftAnchorPoints[1].transform.position, fracjourney);
            OtherScale.transform.position = Vector3.Lerp(rightAnchorPoints[0].transform.position, rightAnchorPoints[1].transform.position, fracjourney);
        }
        else if (amountofboxes == 2)
        {
            float fracjourney = distCovered / Vector3.Distance(leftAnchorPoints[1].transform.position, leftAnchorPoints[2].transform.position);
            transform.position = Vector3.Lerp(leftAnchorPoints[0].transform.position, leftAnchorPoints[2].transform.position, fracjourney);
            OtherScale.transform.position = Vector3.Lerp(rightAnchorPoints[0].transform.position, rightAnchorPoints[2].transform.position, fracjourney);
        }
    }
}

