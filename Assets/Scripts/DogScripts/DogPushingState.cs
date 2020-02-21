using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPushingState : DogState
{
    bool canMoveObject;
    bool movingObject = false;

    GameObject affectedObject;
    public float leftInteractPos = -1f;
    public float rightInteractPos = 1f;
    public float yInteractOffsetAbove = 0.9f; //Ge förklaring för båda.
    public float yInteractOffsetBelow = -0.9f;
    private float interactPosition;



    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject"))
        {
            affectedObject = other.gameObject;

            Vector3 dir = affectedObject.transform.position - dog.transform.position;
            Debug.Log(dir);
            if (dir.y >= yInteractOffsetAbove || dir.y <= yInteractOffsetBelow)
            {
                canMoveObject = false;
            }
            else
            {
                canMoveObject = true;
            }
        }
    }
    /*void HandleMovableObjects()
    {
        if (affectedObject != null)
        {
            if (Input.GetButtonDown("Interact") && canMoveObject && movingObject == false)
            {
                if (affectedObject != null)
                {
                    if (!positionChecked)
                    {
                        if (dog.transform.position.x > affectedObject.transform.position.x)
                        {
                            interactPosition = leftInteractPos;
                        }
                        else
                        {
                            interactPosition = rightInteractPos;
                        }
                        positionChecked = true;
                    }
                    dog.transform.position = new Vector3(affectedObject.transform.position.x - interactPosition, dog.transform.position.y, 0);
                    affectedObject.transform.parent = gameObject.transform;
                    affectedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    dog.rb2d.isKinematic = true;
                    movingObject = true;
                }
            }
            else if (Input.GetButtonDown("Interact") && movingObject)
            {
                affectedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                dog.rb2d.GetComponent<Rigidbody2D>().isKinematic = false;
                affectedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, affectedObject.transform.position.y);
                affectedObject.transform.parent = null;
                positionChecked = false;
                movingObject = false;
                canMoveObject = false;
            }
        }
    }*/

    public override void OnTriggerExit2D(Collider2D other)
    {

    }

}
