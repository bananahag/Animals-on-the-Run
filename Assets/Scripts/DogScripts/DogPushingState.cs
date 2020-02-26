using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    public AudioSource pushSource;
    bool dropBox;
    public bool type1;
    public bool type2;



    [Tooltip("The walking speed of the dog while pushing or pulling a movable object.")]
    public float pushingSpeed = 3.0f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;

        if(!type1 && !type2)
        {
            Debug.LogWarning("Select pushing type in DogPushingState. Either Type1 or Type2 must be selected.");
            type1 = true;
        }
        if (type1 && type2)
        {
            type1 = !type2;
            type2 = !type1;
        }
   }

    public override void Enter()
    {

        if (type1)
        {
            dog.affectedObject.GetComponent<MovableObject>().Pickup(dog.gameObject);
            dog.movingObject = true;
        }
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
        if (type1)
        {
            dog.affectedObject.GetComponent<MovableObject>().Drop();
        }

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
        if(dog.x != 0)
        {
            pushSource.Play();
        }

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
                dog.animator.Play("DogPulling");
            }
            else
            {
                dog.animator.Play("DogPushing");
            }
        }
        if (dog.pushSideIsLeft)
        {
            if(dog.movement.x >= 0)
            {
                dog.animator.Play("DogPushing");
            }
            else
            {
                dog.animator.Play("DogPulling");
            }
        }
    }

}
