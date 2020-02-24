using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    
    bool dropBox;


    [Tooltip("The walking speed of the dog while pushing or pulling a movable object.")]
    public float pushingSpeed = 3.0f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
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
        if (!dog.pushSideIsLeft)
        {
            if(dog.movement.x < 0)
            {
                dog.animator.Play("Pulling");
            }
            else
            {
                dog.animator.Play("Pushing");
            }
        }
        if (dog.pushSideIsLeft)
        {
            if(dog.movement.x >= 0)
            {
                dog.animator.Play("Pushing");
            }
            else
            {
                dog.animator.Play("Pulling");
            }
        }
    }

}
