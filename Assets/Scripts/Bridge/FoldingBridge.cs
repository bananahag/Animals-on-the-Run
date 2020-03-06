﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldingBridge : MonoBehaviour
{

    public GameObject Bridge;
    Rigidbody2D lrb;
    public Rigidbody2D rb;
    
    float startpos;
    float otherstartpos;
    
    bool bridgemoving;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lrb = Bridge.GetComponent<Rigidbody2D>();
        startpos = rb.rotation;
        otherstartpos = lrb.rotation;
    }

    // Update is called once per frame
    void Update()
    {
      

   
    }
   
    public void BridgeFold(float angle, float foldtime, bool active)
    {
        if (active)
        {
            

            if (rb.rotation < angle)
            {
                rb.MoveRotation(Mathf.LerpAngle(startpos, angle, foldtime));
                lrb.MoveRotation(Mathf.LerpAngle(otherstartpos, -1 * angle, foldtime));
            }
            else
            {
                rb.rotation = angle;
                lrb.rotation = rb.rotation * -1;
                startpos = angle;
                otherstartpos = angle * -1;
                
                Debug.Log(startpos + "Erik");
            }
        }
        else if (!active)
        {
            
            if (rb.rotation > 0)
            {
                Debug.Log(startpos);
                rb.MoveRotation(Mathf.LerpAngle(startpos, 0, foldtime));
                lrb.MoveRotation(Mathf.LerpAngle(otherstartpos, 0, foldtime));
            }
            else
            {
                rb.rotation = 0;
                lrb.rotation = rb.rotation * -1;
                startpos = rb.rotation;
                otherstartpos = lrb.rotation;
               
            }
        }
    }
}
