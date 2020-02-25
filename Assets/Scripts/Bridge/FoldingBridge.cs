using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldingBridge : MonoBehaviour
{

    public GameObject Bridge;
    Rigidbody2D lrb;
    public Rigidbody2D rb;
    
    bool Active = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lrb = Bridge.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
      

   
    }
   
    public void BridgeFold(float angle, float foldtime, float clipp , bool active)
    {
        if (active)
        {


            if (rb.rotation < clipp)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, foldtime * Time.deltaTime));
                lrb.MoveRotation(Mathf.LerpAngle(lrb.rotation, -1 * angle, foldtime * Time.deltaTime));
            }
            else
            {
                rb.rotation = angle;
                lrb.rotation = rb.rotation * -1;
            }
        }
        else if (!active)
        {
            if (rb.rotation > clipp)
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, 0, foldtime * Time.deltaTime));
                lrb.MoveRotation(Mathf.LerpAngle(lrb.rotation, 0, foldtime * Time.deltaTime));
            }
            else
            {
                rb.rotation = 0;
                lrb.rotation = rb.rotation * -1;
            }
        }
    }
}
