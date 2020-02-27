using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [Tooltip("The anchor for the left part of the scale")]
    public GameObject leftAnchor;
    [Tooltip("The anchor for the right part of the scale")]
    public GameObject rightAnchor;
    [Tooltip("The right scale which is a part of the scalelift")]
    public GameObject connectedScale;
    public Rigidbody2D rb;

    [Tooltip("Amount of positions the scale will move between, you have to place them out for the left side")]
    public List<Transform> leftAnchorpoints;
    Vector2 leftStartpos;

    [HideInInspector]
    public int amountofboxes;
    [Tooltip("Sets the speed of the lift")]
    public float speed = 1.0f;

    float startTime;
    bool movingDown = false;
    bool movingUP = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftStartpos = leftAnchor.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        UpdateScale(distCovered);
        
    }
    private void FixedUpdate()
    {
        
        rightAnchor.GetComponent<Rigidbody2D>().velocity = -rb.velocity;
    }
    void UpdateScale(float distCovered)
    {
        if (amountofboxes < 0)
        {
            amountofboxes = 0;

        }
        else if (amountofboxes > leftAnchorpoints.Count)
        {
            amountofboxes = leftAnchorpoints.Count;
        }
        if (movingDown)
        {
            if (amountofboxes == 1)
            {
                float fracjourney = distCovered / Vector3.Distance(leftStartpos, leftAnchorpoints[0].position);
                leftAnchor.transform.position = Vector3.Lerp(leftStartpos, leftAnchorpoints[0].position, fracjourney);
            }
            else
            {
               
                float fracjourney = distCovered / Vector3.Distance(leftAnchorpoints[amountofboxes -2].position, leftAnchorpoints[amountofboxes - 1].position);
                leftAnchor.transform.position = Vector3.Lerp(leftAnchorpoints[amountofboxes - 2].position, leftAnchorpoints[amountofboxes - 1].position, fracjourney);
                
            }
        }

        else if (movingUP)
        {
            if (amountofboxes == 0)
            {
                float fracjourney = distCovered / Vector3.Distance(leftAnchorpoints[0].position, leftStartpos);
                leftAnchor.transform.position = Vector3.Lerp(leftAnchorpoints[0].position, leftStartpos, fracjourney);
            }
            else
            {
                float fracjourney = distCovered / Vector3.Distance(leftAnchorpoints[amountofboxes].position, leftAnchorpoints[amountofboxes - 1].position);
                leftAnchor.transform.position = Vector3.Lerp(leftAnchorpoints[amountofboxes].position, leftAnchorpoints[amountofboxes - 1].position, fracjourney);
            }
        }
    }
    public void DownwardsVelocity()
    {
        movingDown = true;
        movingUP = false;
        float startTime = Time.time;
    }
    public void UpwardsVelocity()
    {
        movingUP = true;
        movingDown = false;
        float startTime = Time.time;
    }
}
