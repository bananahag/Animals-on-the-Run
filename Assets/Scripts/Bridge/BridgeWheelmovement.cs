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
    float fraction;
    
    void Start()
    {
        bridge = GameObject.Find("Right Bridge").GetComponent<FoldingBridge>();
       
    }
    public void DraiSpakenKronk()
    {
        timer = spinnTime;
        accelerationtimer = accelerationTime;
        active = !active;
        fraction = 0;
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
       
    }

    private void FixedUpdate()
    {
        accelerationtimer -= Time.deltaTime;
        timer -= Time.deltaTime;
        
        if (active)
        {
            if (timer < 0)
            {
                fraction += Time.deltaTime / foldtime;
                bridge.BridgeFold(angle, fraction, active);
            }
            RotateWheel(active);
       
        }
        else if (!active)
        {

            if (timer < 0)
            {
                fraction += Time.deltaTime / foldtime;
                bridge.BridgeFold(angle, fraction, active);

            }
            RotateWheel(active);
        }
    }
        
}


