using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    
    bool dropBox;
    bool rightSide;
    bool leftSide;

    [Tooltip("The walking speed of the dog while pushing or pulling a movable object.")]
    public float pushingSpeed = 3.0f;

    [Tooltip("A offset for the box for what is considered above the box for the dog. Is used to limit the dog to pushing and pulling from the sides of the box")]
    public float yInteractOffsetAbove = 0.9f; //Ge förklaring för båda.
    [Tooltip("A offset for the box for what is considered below the box for the dog. Is used to limit the dog to pushing and pulling from the sides of the box")]
    public float yInteractOffsetBelow = -0.9f;


    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        Debug.Log("Dog In Pushing State");
        dog.affectedObject.GetComponent<MovableObject>().Pickup(dog.gameObject);
        dog.movingObject = true;
    }
    public override void Update()
    {
        CheckInput();
        PlayAnimations();
    }

    public override void Exit()
    {
        dog.movingObject = false;
        dog.canMoveObject = false;
        dropBox = false;
        dog.affectedObject.GetComponent<MovableObject>().Drop();

    }

    void CheckInput()
    {
        if(Input.GetButtonDown("Interact") && dog.movingObject)
        {
            dropBox = true;
        }
    }

    public override void FixedUpdate()
    {
        dog.movement = new Vector2(dog.x * pushingSpeed, dog.rb2d.velocity.y);

        if(dropBox && dog.grounded)
        {
            dog.ChangeState(dog.groundedState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject"))
        {
            dog.affectedObject = other.gameObject;
            //Flytta ut någon annanstans funkar inte att ha här
            Vector3 dir = dog.affectedObject.transform.position - dog.transform.position;
            Debug.Log(dir);
            if (dir.y >= yInteractOffsetAbove || dir.y <= yInteractOffsetBelow)
            {
                dog.canMoveObject = false;
            }
            else
            {
                dog.canMoveObject = true;
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject"))
        {
            dropBox = true;
            dog.affectedObject.GetComponent<MovableObject>().Drop();
        }
    }
    public void PlayAnimations()
    {
        if (leftSide)
        {
            if(dog.transform.position.x < dog.affectedObject.transform.position.x)
            {
                dog.animator.Play("Pulling");
                Debug.Log("PULL");
            }
            else
            {
                dog.animator.Play("Pushing");
                Debug.Log("PUSH");
            }
        }
        if (rightSide)
        {
            if(dog.transform.position.x < dog.affectedObject.transform.position.x)
            {
                dog.animator.Play("Pushing");
                Debug.Log("PUSH");
            }
            else
            {
                dog.animator.Play("Pulling");
                Debug.Log("PULL");

            }
        }
    }

}
