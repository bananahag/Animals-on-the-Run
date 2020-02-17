using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    public GameObject leftAnchor;
    Vector2 leftStartpos;
    public GameObject rightAnchor;
    public GameObject connectedScale;
    Vector2 rightStartpos;
    [Tooltip("Gamer juice")]
    public List<Transform> leftAnchorpoints;
    public List<Transform> rightAnchorpoints;
    public int amountofboxes;
    float startTime;
    public float speed = 1.0f;
    bool notCalled;
    public Rigidbody2D rb;
    bool movingDown = false;
    bool movingUP = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftStartpos = leftAnchor.transform.position;
        rightStartpos = rightAnchor.transform.position;
        
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
        if (movingDown)
        {
            if (amountofboxes == 1)
            {
                float fracjourney = distCovered / Vector3.Distance(leftStartpos, leftAnchorpoints[0].position);
                leftAnchor.transform.position = Vector3.Lerp(leftStartpos, leftAnchorpoints[0].position, fracjourney);
            }
            else if (amountofboxes == 2)
            {
                float fracjourney = distCovered / Vector3.Distance(leftAnchorpoints[0].position, leftAnchorpoints[1].position);
                leftAnchor.transform.position = Vector3.Lerp(leftAnchorpoints[0].position, leftAnchorpoints[1].position, fracjourney);
            }
        }

        else if (movingUP)
        {
            if (amountofboxes == 0)
            {
                float fracjourney = distCovered / Vector3.Distance(leftAnchorpoints[0].position, leftStartpos);
                leftAnchor.transform.position = Vector3.Lerp(leftAnchorpoints[0].position, leftStartpos, fracjourney);
            }
            else if (amountofboxes == 1)
            {
                float fracjourney = distCovered / Vector3.Distance(leftAnchorpoints[1].position, leftAnchorpoints[0].position);
                leftAnchor.transform.position = Vector3.Lerp(leftAnchorpoints[1].position, leftAnchorpoints[0].position, fracjourney);
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
