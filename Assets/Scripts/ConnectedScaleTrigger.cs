using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedScaleTrigger : MonoBehaviour
{
    public GameObject leftSideScale;
    Scale scale;

    private void Start()
    {
        
        scale = leftSideScale.GetComponent<Scale>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovableObject")
        {
            scale.amountofboxes--;
            scale.UpwardsVelocity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MovableObject")
        {
            scale.amountofboxes++;
            scale.DownwardsVelocity();
        }
    }
}
