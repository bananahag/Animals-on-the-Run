using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerScale : MonoBehaviour
{
    Scale scale;

    private void Start()
    {
        scale = GetComponentInParent<Scale>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovableObject")
        {
            scale.amountofboxes++;
            scale.DownwardsVelocity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MovableObject")
        {
            scale.amountofboxes--;
            scale.UpwardsVelocity();
        }
    }
}
