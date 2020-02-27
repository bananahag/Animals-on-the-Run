using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogGroundedState : DogState
{
    [Tooltip("Audio source that plays when the dog takes a step on the ground.")]
    public AudioSource stepSource;

    public float walkingSpeed = 4.0f;

    [Tooltip("A offset for the movable box for what is considered above the box for the dog. Is used to limit the dog to pushing and pulling from the sides of the box")]
    public float yInteractOffsetAbove = 0.9f; //Ge förklaring för båda.
    [Tooltip("A offset for the movable box for what is considered below the box for the dog. Is used to limit the dog to pushing and pulling from the sides of the box")]
    public float yInteractOffsetBelow = -0.9f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        dog.animator.Play("DogIdle");
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (dog.active)
        {
            CheckInput();
        }

        if (dog.x > 0)
        {
            dog.facingRight = true;
        }
        else if (dog.x < 0)
        {
            dog.facingRight = false;
        }

        GroundedAnimations();

        if (!dog.grounded)
        {
            dog.ChangeState(dog.inAirState);
        }
    }

    public override void FixedUpdate()
    {
        dog.movement = new Vector2(dog.x * walkingSpeed, dog.rb2d.velocity.y);

        if (dog.movingObject)
        {
            dog.ChangeState(dog.pushingState);
        }

        if (dog.swimming)
        {
            dog.ChangeState(dog.swimmingState);
        }
        CheckIfFalling();
    }

    public void CheckInput()
    {
        if (Input.GetButtonDown("Jump") || dog.jumpBuffer)
        {
            dog.ChangeState(dog.jumpsquatState);
        }
        if(Input.GetButtonDown("Interact") && dog.canMoveObject)
        {
            dog.ChangeState(dog.pushingState);
        }
        if (Input.GetButtonDown("Interact") && dog.closeToHuman && !dog.canMoveObject) //Om både människa och låda går att interagera med prioriteras lådan
        {
            dog.ChangeState(dog.charmingState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject"))
        {
            dog.affectedObject = other.gameObject;
            Vector3 dir = dog.affectedObject.transform.position - dog.transform.position;
            Debug.Log(dir);
            if (dir.y >= yInteractOffsetAbove || dir.y <= yInteractOffsetBelow)
            {
                if (dog.pushingState.type1)
                {
                    dog.affectedObject.GetComponent<MovableObject>().canMoveObject = false;
                }
                else if (dog.pushingState.type2)
                {
                    dog.canMoveObject = false; ;
                }
            }
            else
            {
                if (dir.x <= 0)
                {
                    if (dog.pushingState.type1)
                    {
                        dog.affectedObject.GetComponent<MovableObject>().canMoveObject = true;
                    } else if (dog.pushingState.type2)
                    {
                        dog.canMoveObject = true;
                    }
                    dog.pushSideIsLeft = true;
                }
                else if (dir.x > 0)
                {
                    if (dog.pushingState.type1)
                    {
                        dog.affectedObject.GetComponent<MovableObject>().canMoveObject = true;
                    } else if (dog.pushingState.type2)
                    {
                        dog.canMoveObject = true;
                    }
                    dog.pushSideIsLeft = false;
                }
                dog.canMoveObject = true;
            }
        }
        if (other.gameObject.tag == "Finish")
        {
            dog.levelCompleted = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            dog.human = other.gameObject;
            dog.closeToHuman = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            dog.swimming = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject") && dog.affectedObject == null)
        {
            if (dog.pushingState.type1)
            {
                dog.affectedObject.GetComponent<MovableObject>().canMoveObject = false;
            } else if (dog.pushingState.type2)
            {
                dog.canMoveObject = false;
            }
            dog.affectedObject = null;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            dog.human = null;
            dog.closeToHuman = false;
        }
    }

    public void GroundedAnimations()
    {
        if (dog.movement.x != 0)
        {
            dog.animator.Play("DogWalking");
        }
        if (dog.movement.x == 0)
        {
            dog.animator.Play("idle");
        }
    }

    void CheckIfFalling()
    {
        if (!dog.grounded && !dog.swimming)
        {
            dog.ChangeState(dog.inAirState);
        }
    }
    public void PlayStepSound()
    {
        stepSource.Play();
    }


}
