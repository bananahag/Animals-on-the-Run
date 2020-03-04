using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    public AudioSource pushSource;
    public bool dropBox;
    public bool underBox;


    [Tooltip("The walking speed of the dog while pushing or pulling a movable object.")]
    public float pushingSpeed = 3.0f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
   }

    public override void Enter()
    {

        //dog.affectedObject.GetComponent<MovableObject>().Pickup(dog.gameObject);
        //dog.canMoveObject = false;
        //dog.movingObject = true;
        dog.affectedObject.GetComponent<FixedJoint2D>().connectedBody = dog.rb2d;
        dog.affectedObject.GetComponent<FixedJoint2D>().enabled = true;
        dog.affectedObject.GetComponent<MovableObject>().beingMoved = true;


        dog.movingObject = true;
        dropBox = false;
    }
    public override void Update()
    {
        CheckInput();
        PlayAnimations();

    }

    public override void Exit()
    {
        //dog.canMoveObject = false;
        //dog.affectedObject = null;
        //dog.affectedObject.GetComponent<MovableObject>().Drop();
        dog.movingObject = false;
        Debug.Log("Exit pushing");
        dog.affectedObject.GetComponent<MovableObject>().beingMoved = false;
        dog.StartCoroutine(dog.MoveObjectCoolDown());
    }

    void CheckInput()
    {
        
        if(Input.GetButtonDown("Interact") /*&& dog.movingObject*/)
        {
            dropBox = true;
            dog.affectedObject.GetComponent<FixedJoint2D>().enabled = false;
        }

    }

    public override void FixedUpdate()
    {
        dog.movement = new Vector2(dog.x * pushingSpeed, dog.rb2d.velocity.y);
        if (dog.affectedObject != null)
        {
            if (dog.x != 0)
            {
                pushSource.Play();
            }

            if (dog.affectedObject != null)

            {
                /*if (dog.affectedObject.GetComponent<MovableObject>().grounded == false)
                {
                    dropBox = true;
                }*/
            }


            if (dropBox)
            {
                dog.affectedObject.GetComponent<FixedJoint2D>().enabled = false;
                if (dog.swimming)
                {
                    dog.ChangeState(dog.swimmingState);
                }
                if (dog.grounded)
                {
                    dog.ChangeState(dog.groundedState);
                }
            }

            /* if (underBox)
             {
                 dropBox = true;
             }
             */
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject"))
        {

            //dropBox = true;
            //dog.affectedObject.GetComponent<MovableObject>().Drop();
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
