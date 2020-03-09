using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeWheelmovement : MonoBehaviour
{
    [Tooltip("Drag and drop other bridge wheel else it wont work")]
    public GameObject otherBridgeWheel;
    [Tooltip("Drag and drop the other bridge parent else it wont work")]
    public GameObject otherbridge;
    FoldingBridge bridge;
    public bool active = false;
    [Tooltip("The amount of degrees in tilt when done")]
    public float angle = 1;
   [Tooltip("Total time for bridge to reach x degrees")]
    public float foldtime = 1.0f;
    float spinnSpeed = 0;
    [Tooltip("says its self, doesnt really work as intended but its something")]
    public float accelerationSpeed;
    [Tooltip("says its self, doesnt really work as intended but its something")]
    public float accelerationTime;
    [Tooltip("End speed for wheel spinn")]
    public float maxSpinnSpeed;
    [Tooltip("how many seconds the wheels spinns before bridge folds")]
    public float spinnTime = 1.0f;
    float timer;
    float accelerationtimer;
    float fraction;
    public AudioSource bridgeSoundSource;
   
   
    
    void Start()
    {
        bridge = otherbridge.GetComponent<FoldingBridge>();
  
    }
    public void DraiSpakenKronk()
    {
        if (!bridge.bridgemoving)
        {
            timer = spinnTime;
            accelerationtimer = accelerationTime;
            active = !active;
            fraction = 0;
            bridgeSoundSource.Play();
        }
          
        
       
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
            
            if (bridge.rb.rotation > 0)
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


