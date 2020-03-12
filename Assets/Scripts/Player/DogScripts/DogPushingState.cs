using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    public AudioSource pushSource;
    public AudioSource pullRopeSource;
    [HideInInspector]
    public bool dropBox;
    [HideInInspector]
    public bool boxGrounded;

    bool canPlayPushSource;

    float startPosX, dragDistance, pushDistance;//All of these are related to the rope pulling


    [Tooltip("The walking speed of the dog while pushing or pulling a movable object.")]
    public float pushingSpeed = 3.0f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        startPosX = dog.transform.position.x;
        pushSource.loop = true;
        pullRopeSource.loop = true;
        canPlayPushSource = true;
        if (!dog.pullingRope)
        {
            dog.affectedObject.GetComponent<FixedJoint2D>().connectedBody = dog.rb2d;
            dog.affectedObject.GetComponent<FixedJoint2D>().enabled = true;
            dog.affectedObject.GetComponent<MovableObject>().beingMoved = true;
        }
        else
        {
            dog.rope.transform.SetParent(dog.transform);
            dragDistance = Mathf.Abs(dog.rope.GetComponent<StallRope>().transform.position.x - dog.rope.GetComponent<StallRope>().targetPos.x);
            pushDistance = dog.rope.GetComponent<StallRope>().dragDistance - dragDistance;
        }


        dog.movingObject = true;
        dropBox = false;
    }
    public override void Update()
    {
        CheckInput();
        PlayAnimations();

        if(dog.affectedObject != null && !dog.pullingRope)
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
        pullRopeSource.Stop();
        canPlayPushSource = true;
        if (!dog.pullingRope)
            dog.affectedObject.GetComponent<MovableObject>().beingMoved = false;
        else
            dog.rope.transform.SetParent(null);
        dog.pullingRope = false;
        dog.movingObject = false;
        Debug.Log("Exit pushing");
        

        dog.StartCoroutine(dog.MoveObjectCoolDown());
    }

    void CheckInput()
    {
        
        if(Input.GetButtonDown("Interact") && !dog.pullingRope)
        {
            dropBox = true;
            dog.affectedObject.GetComponent<FixedJoint2D>().enabled = false;
        }
        else if (Input.GetButtonDown("Interact") && dog.pullingRope)
        {
            dog.rope.GetComponent<StallRope>().dogIsPulling = false;
            dog.closeToRope = false;
            dog.ChangeState(dog.groundedState);
        }

    }

    public override void FixedUpdate()
    {
        if (dog.pullingRope && dog.transform.position.x <= (startPosX - dragDistance) && dog.x < 0.0f)
        {
            dog.transform.position = new Vector3(startPosX - dragDistance, dog.transform.position.y, dog.transform.position.z);
            dog.movement = new Vector2(0.0f, dog.rb2d.velocity.y);
        }
        else if (dog.pullingRope && dog.transform.position.x >= (pushDistance + startPosX) && dog.x > 0.0f)
        {
            pullRopeSource.Stop();
            dog.transform.position = new Vector3(startPosX + pushDistance, dog.transform.position.y, dog.transform.position.z);
            dog.movement = new Vector2(0.0f, dog.rb2d.velocity.y);
        }
        else
            dog.movement = new Vector2(dog.x * pushingSpeed, dog.rb2d.velocity.y);

        if (dog.affectedObject != null && !dog.pullingRope)
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

        if (dog.pullingRope)
        {
            if (!pullRopeSource.isPlaying && dog.x < 0.0f)
                canPlayPushSource = true;
            if (dog.x != 0)
            {
                if (canPlayPushSource)
                    pullRopeSource.Play();
                canPlayPushSource = false;
            }
            else
            {
                pullRopeSource.Stop();
                canPlayPushSource = true;
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
