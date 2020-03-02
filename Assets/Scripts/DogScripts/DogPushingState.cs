using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    public AudioSource pushSource;
    public bool dropBox;
    public bool type1;
    [Tooltip("")]
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

        if (type2)
        {
            if(dog.affectedObject.transform.position.x > dog.transform.position.x)
            {
                dog.affectedObject.transform.position = dog.transform.position + new Vector3(dog.radius, 0, 0);
            }
            else if(dog.affectedObject.transform.position.x <= dog.transform.position.x)
            {
                dog.affectedObject.transform.position = dog.transform.position - new Vector3(dog.radius, 0, 0);
            }
        }
        dropBox = false;
    }
    public override void Update()
    {
        CheckInput();
        PlayAnimations();
        if (type2)
        {
            if (dog.affectedObject.transform.position.x > dog.transform.position.x)
            {
                dog.affectedObject.transform.position = dog.transform.position + new Vector3(dog.radius, 0, 0);
            }
            else if (dog.affectedObject.transform.position.x <= dog.transform.position.x)
            {
                dog.affectedObject.transform.position = dog.transform.position - new Vector3(dog.radius, 0, 0);
            }
        }
    }

    public override void Exit()
    {
        dog.movingObject = false;
        dog.canMoveObject = false;
        //if (type1)
        //{
            dog.affectedObject.GetComponent<MovableObject>().Drop();
        //}
        Debug.Log("Exit pushing");
        dog.affectedObject = null;

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
        if (dog.x != 0)
        {
            pushSource.Play();
        }

        if (dropBox && dog.grounded)
        {
            dog.ChangeState(dog.groundedState);
        }

        if (dog.affectedObject != null)

        {
            if (dog.affectedObject.GetComponent<MovableObject>().grounded == false)
            {
                dropBox = true;
            }
        }

        if(dropBox && dog.swimming)
        {
            dog.ChangeState(dog.swimmingState);
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
