using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    public AudioSource pushSource;
    [HideInInspector]
    public bool dropBox;
    [HideInInspector]
    public bool boxGrounded;

    bool canPlayPushSource;


    [Tooltip("The walking speed of the dog while pushing or pulling a movable object.")]
    public float pushingSpeed = 3.0f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        pushSource.loop = true;
        canPlayPushSource = true;
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

        if(dog.affectedObject != null)
        {
            if (!dog.grounded || !dog.affectedObject.GetComponent<MovableObject>().BoxGrounded())
            {
                dropBox = true;
                dog.affectedObject.GetComponent<FixedJoint2D>().enabled = false;
            }
        }
    }

    public override void Exit()
    {
        pushSource.Stop();
        canPlayPushSource = true;
        dog.movingObject = false;
        Debug.Log("Exit pushing");
        dog.affectedObject.GetComponent<MovableObject>().beingMoved = false;
        dog.StartCoroutine(dog.MoveObjectCoolDown());
    }

    void CheckInput()
    {
        
        if(Input.GetButtonDown("Interact"))
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
                if (canPlayPushSource)
                    pushSource.Play();
                canPlayPushSource = false;
            }
            else
            {
                pushSource.Stop();
                canPlayPushSource = true;
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
        }
    }

    public void PlayAnimations()
    {
        if (dog.facingRight)
        {
            if (dog.x < 0)
            {
                dog.animator.Play("DogPulling");
            }
            else if (dog.x > 0)
            {
                dog.animator.Play("DogPushing");
            }
        }
        else if (!dog.facingRight)
        {
            if (dog.x < 0)
            {
                dog.animator.Play("DogPushing");
            }
            else if (dog.x > 0)
            {
                dog.animator.Play("DogPulling");
            }
        }
    }

}
