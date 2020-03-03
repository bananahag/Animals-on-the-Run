using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerScale : MonoBehaviour
{
    public GameObject otherScale;
    NewScale newScale;
    private void Start()
    {
        newScale = GetComponentInParent<NewScale>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case ("MovableObject"):
                newScale.amountBoxes++;
                otherScale.GetComponent<NewScale>().amountBoxes--;
                newScale.MoveScaleDown();
                otherScale.GetComponent<NewScale>().MoveScaleDown();
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case ("MovableObject"):
                newScale.amountBoxes--;
                otherScale.GetComponent<NewScale>().amountBoxes++;
                newScale.MoveScaleDown();
                otherScale.GetComponent<NewScale>().MoveScaleDown();
                break;
            default:
                break;
        }

    }
}
