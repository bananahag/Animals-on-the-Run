using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallRope : MonoBehaviour
{
    [Tooltip("The stall object the rope is connected to.")]
    public GameObject stall;
    [Tooltip("The distance the rope needs to be dragged before the stall activates.")]
    public float dragDistance = 0.5f;
    [Tooltip("The amount of time (in seconds) the stall will be activeted.")]
    public float stallIsActiveTime = 5.0f;

    Vector2 startPos, targetPos;
    bool stallIsActivated;

    float fraction, timePassed;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPos = new Vector2(startPos.x - dragDistance, startPos.y);
        timePassed = 0.0f;
        fraction = 0.0f;
        stallIsActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= targetPos.x && !stallIsActivated)
        {
            transform.position = targetPos;
            timePassed = 0.0f;
            fraction = 0.0f;
            stall.GetComponent<Stall>().ChangeState(true);
            stallIsActivated = true;
        }

        if (stallIsActivated)
            MoveBackRope();
    }

    void MoveBackRope()
    {
        timePassed = 0.0f;
        fraction += Time.deltaTime / stallIsActiveTime;
        transform.position = Vector2.Lerp(targetPos, startPos, fraction);
        if (fraction >= 1.0f)
        {
            stall.GetComponent<Stall>().ChangeState(false);
            stallIsActivated = false;
        }
    }
}
