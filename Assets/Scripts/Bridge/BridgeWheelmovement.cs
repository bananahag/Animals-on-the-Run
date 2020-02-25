using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeWheelmovement : MonoBehaviour
{

    public GameObject otherBridgeWheel;
    FoldingBridge bridge;
    public bool active = false;
    public float angle = 1;
    public float clipAngle;
    public float foldtime = 1.0f;
    float spinnSpeed = 0;
    public float accelerationSpeed;
    public float accelerationTime;
    public float maxSpinnSpeed;
    public float spinnTime = 1.0f;
    float timer;
    float accelerationtimer;
    float newangle;
    void Start()
    {
        bridge = GameObject.Find("Right Bridge").GetComponent<FoldingBridge>();
       
    }
    public void DraiSpakenKronk()
    {
        timer = spinnTime;
        accelerationtimer = accelerationTime;
        active = !active;
    }

    void RotateWheel(bool active)
    {
        if (active)
        {

        if (bridge.rb.rotation < angle)
        {
            if (accelerationSpeed < maxSpinnSpeed && accelerationtimer < 0)
            {
                accelerationSpeed += accelerationSpeed * 0.5f;
                accelerationtimer = accelerationTime;
            }
            else if (accelerationSpeed >= maxSpinnSpeed)
            {
                accelerationSpeed = maxSpinnSpeed;
            }
            transform.Rotate(0, 0, -1 * accelerationSpeed * Time.deltaTime);
            otherBridgeWheel.transform.Rotate(0, 0, accelerationSpeed * Time.deltaTime);
        }
        }
        else if (!active)
        {
            if (bridge.rb.rotation > newangle)
            {
                if (accelerationSpeed < maxSpinnSpeed && accelerationtimer < 0)
                {
                    accelerationSpeed += accelerationSpeed * 0.5f;
                    accelerationtimer = accelerationTime;
                }
                else if (accelerationSpeed >= maxSpinnSpeed)
                {
                    accelerationSpeed = maxSpinnSpeed;
                }
                transform.Rotate(0, 0, accelerationSpeed * Time.deltaTime);
                otherBridgeWheel.transform.Rotate(0, 0, -1 * accelerationSpeed * Time.deltaTime);
            }
        }
    }
    void Update()
    {
        Debug.Log(timer);
    }

    private void FixedUpdate()
    {
        accelerationtimer -= Time.deltaTime;
        if (active)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                bridge.BridgeFold(angle, foldtime, clipAngle, active);
            }
            RotateWheel(active);
       
        }
        else if (!active)
        {
           
           newangle = angle - angle;
            float newclipAngle = angle - clipAngle;
            if (timer < 0)
            {
            bridge.BridgeFold(newangle, foldtime, newclipAngle, active);

            }
            RotateWheel(active);
        }
    }
        
}


