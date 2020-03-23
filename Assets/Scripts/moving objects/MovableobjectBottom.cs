using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableobjectBottom : MonoBehaviour
{
    public float raycastlength;
    GameObject scalehit;

   public Vector3 leftoffset = new Vector3(-.7f, -.7f, 0);
   public Vector3 rightoffest = new Vector3(.7f, -.7f, 0);
    public bool leavingGround;



    private void OnTriggerEnter2D(Collider2D collision)
    {

        bool lefthit = false;
        bool righthit = false;
        
        if (collision.gameObject.tag == "MovableObject" || collision.gameObject.tag == "Scale")
        {
            leavingGround = false;
            RaycastHit2D[] lefthits = Physics2D.RaycastAll(transform.position + leftoffset, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in lefthits)
            {
                if (hit.collider.tag == "Scale")
                {
                    lefthit = true;
                    scalehit = hit.collider.gameObject;
                }
                
            }
            RaycastHit2D[] righthits = Physics2D.RaycastAll(transform.position + rightoffest, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in righthits)
            {
                if (hit.collider.tag == "Scale")
                {
                    righthit = true;
                    scalehit = hit.collider.gameObject;
                }
                

            }

            if (righthit || lefthit)
            {
                
                scalehit.GetComponent<NewScale>().MoveScaleDown();
                scalehit.GetComponent<NewScale>().OtherScale.GetComponent<NewScale>().MoveScaleUp();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        bool lefthit = false;
        bool righthit = false;
        
        if (collision.gameObject.tag == "MovableObject" || collision.gameObject.tag == "Scale")
        {

            RaycastHit2D[] lefthits = Physics2D.RaycastAll(transform.position + leftoffset, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in lefthits)
            {
                leavingGround = true;
                print("Leaving");
                if (hit.collider.tag == "Scale")
                {
                    lefthit = true;
                    scalehit = hit.collider.gameObject;
                }
               
            }
            RaycastHit2D[] righthits = Physics2D.RaycastAll(transform.position + rightoffest, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in righthits)
            {
                if (hit.collider.tag == "Scale")
                {
                    righthit = true;
                    scalehit = hit.collider.gameObject;
                }
                

            }
            if (righthit || lefthit)
            {
                scalehit.GetComponent<NewScale>().MoveScaleUp();
                scalehit.GetComponent<NewScale>().OtherScale.GetComponent<NewScale>().MoveScaleDown();
            }

        }
    }
   
    private void OnDrawGizmos()
    {

      
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + leftoffset , (transform.position + leftoffset - new Vector3(0,raycastlength,0)));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + rightoffest, (transform.position + rightoffest - new Vector3(0, raycastlength, 0)));
    }
}
