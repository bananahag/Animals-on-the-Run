using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stall : MonoBehaviour
{
    public enum StallColor {Blue, Green, Purple, Lime, Pink}
    public StallColor stallColor;

    EdgeCollider2D boxCol2D;
    Animator animator;

    bool testing;//REMOVE LATER;

    // Start is called before the first frame update
    void Start()
    {
        boxCol2D = GetComponent<EdgeCollider2D>();
        boxCol2D.enabled = false;
        animator = GetComponent<Animator>();

        switch(stallColor)
        {
            case StallColor.Blue:
                animator.Play("Blue Stall Closed");
                break;
            case StallColor.Green:
                animator.Play("Green Stall Closed");
                break;
            case StallColor.Purple:
                animator.Play("Purple Stall Closed");
                break;
            case StallColor.Lime:
                animator.Play("Lime Stall Closed");
                break;
            case StallColor.Pink:
                animator.Play("Pink Stall Closed");
                break;
        }
    }

    public void ChangeState(bool active)
    {
        if (active)
        {
            switch (stallColor)
            {
                case StallColor.Blue:
                    animator.Play("Blue Stall Opening");
                    break;
                case StallColor.Green:
                    animator.Play("Green Stall Opening");
                    break;
                case StallColor.Purple:
                    animator.Play("Purple Stall Opening");
                    break;
                case StallColor.Lime:
                    animator.Play("Lime Stall Opening");
                    break;
                case StallColor.Pink:
                    animator.Play("Pink Stall Opening");
                    break;
            }
        }
        else
        {
            switch (stallColor)
            {
                case StallColor.Blue:
                    animator.Play("Blue Stall Closing");
                    break;
                case StallColor.Green:
                    animator.Play("Green Stall Closing");
                    break;
                case StallColor.Purple:
                    animator.Play("Purple Stall Closing");
                    break;
                case StallColor.Lime:
                    animator.Play("Lime Stall Closing");
                    break;
                case StallColor.Pink:
                    animator.Play("Pink Stall Closing");
                    break;
            }
        }

        boxCol2D.enabled = active;
    }

    //TESTING PURPOSES
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            testing = !testing;
            ChangeState(testing);
        }
    }
}
