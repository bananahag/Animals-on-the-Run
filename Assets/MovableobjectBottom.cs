using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableobjectBottom : MonoBehaviour
{
    public float raycastlength;

    Vector3 leftoffset = new Vector3(-.7f, -.7f, 0);
    Vector3 rightoffest = new Vector3(.7f, -.7f, 0);
    



    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit");
        bool lefthit = false;
        bool righthit = false;
        leftoffset = new Vector3(-.7f, -.5f, 0);
        rightoffest = new Vector3(.7f, -.5f, 0);
        if (collision.gameObject.tag == "MovableObject" || collision.gameObject.tag == "Scale")
        {
            
            RaycastHit2D[] lefthits = Physics2D.RaycastAll(transform.position + leftoffset, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in lefthits)
            {
                if (hit.collider.tag == "Scale")
                {
                    lefthit = true;
                }
                if (righthit || lefthit)
                {
                    hit.collider.gameObject.GetComponent<NewScale>().MoveScaleDown();
                    hit.collider.gameObject.GetComponent<NewScale>().OtherScale.GetComponent<NewScale>().MoveScaleUp();
                }
            }
            RaycastHit2D[] righthits = Physics2D.RaycastAll(transform.position + rightoffest, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in righthits)
            {
                if (hit.collider.tag == "Scale")
                {
                    righthit = true;
                }
                if (righthit || lefthit)
                {
                    hit.collider.gameObject.GetComponent<NewScale>().MoveScaleDown();
                    hit.collider.gameObject.GetComponent<NewScale>().OtherScale.GetComponent<NewScale>().MoveScaleUp();
                }

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
                print("Leaving");
                if (hit.collider.tag == "Scale")
                {
                    lefthit = true;
                }
                if (righthit || lefthit)
                {
                    hit.collider.gameObject.GetComponent<NewScale>().MoveScaleUp();
                    hit.collider.gameObject.GetComponent<NewScale>().OtherScale.GetComponent<NewScale>().MoveScaleDown();
                }
            }
            RaycastHit2D[] righthits = Physics2D.RaycastAll(transform.position + rightoffest, Vector2.down, raycastlength);
            foreach (RaycastHit2D hit in righthits)
            {
                if (hit.collider.tag == "Scale")
                {
                    righthit = true;
                }
                 if (righthit || lefthit)
                 {
                hit.collider.gameObject.GetComponent<NewScale>().MoveScaleUp();
                hit.collider.gameObject.GetComponent<NewScale>().OtherScale.GetComponent<NewScale>().MoveScaleDown();
                 }

            }
            

        }
    }
   
    private void OnDrawGizmos()
    {

        Vector3 leftoffset = new Vector3(-.7f, -.5f, 0);
        Vector3 rightoffest = new Vector3(.7f, -.5f, 0);
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + leftoffset, new Vector2((transform.position.x - 0.7f), (transform.position.y - 0.5f) - raycastlength));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + rightoffest, new Vector2((transform.position.x + 0.7f), (transform.position.y - 0.5f) - raycastlength));
    }
}
