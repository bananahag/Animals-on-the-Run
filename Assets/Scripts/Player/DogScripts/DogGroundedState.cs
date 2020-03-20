using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[System.Serializable]
public class DogGroundedState : DogState
{
    public float boxInteractDistance = 0.5f;
    public LayerMask boxMask;
    [Tooltip("Audio source that plays the audio clips.")]
    public AudioSource stepSource;
    [Tooltip("Audio clips that plays when the dog takes a step on the ground.")]
    public AudioClip[] stepClips;
    [Tooltip("The minimum and the maximum volume of the footstep sounds.")]
    public float minVolume = 0.9f, maxVolume = 1.1f;
    [Tooltip("The minimum and the maximum pitch of the footstep sounds.")]
    public float minPitch = 0.9f, maxPitch = 1.1f;

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
        if (dog.landing)
        {
            dog.animator.Play("Landing");
        }
        else
        {
            dog.animator.Play("idle");
        }
        dog.canMoveObject = true;
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitBox = Physics2D.Raycast(dog.transform.position, Vector2.right * dog.direction * dog.transform.localScale.x, boxInteractDistance);
        if(hitBox.collider != null)
        {
            if (hitBox.collider.CompareTag("MovableObject") && Input.GetButtonDown("Interact") && dog.canMoveObject)
            {
                dog.affectedObject = hitBox.collider.gameObject;
                dog.ChangeState(dog.pushingState);
            }
            else
            {
                dog.affectedObject = null;
            }
        }

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

        if (Input.GetButtonDown("Light")) //Om både människa och låda går att interagera med prioriteras lådan
        {
            dog.ChangeState(dog.charmingState);
        }
        else if (Input.GetButtonDown("Interact") && dog.closeToRope) //Om både människa och rep går att interagera med prioriteras människan (lådan är fortfarande högsta prio)
        {
            if (dog.rope != null && dog.rope.transform.position.x > dog.transform.position.x)
            {
                dog.pullingRope = true;
                dog.ChangeState(dog.pushingState);
                dog.rope.GetComponent<StallRope>().dogIsPulling = true;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Finish")
        {
            dog.levelCompleted = true;
        }

     /*   if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            
            dog.human = other.gameObject;
            dog.closeToHuman = true;
        }*/

        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            
            dog.swimming = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
           
            dog.human = null;
            dog.closeToHuman = false;
        }
    }

    public void GroundedAnimations()
    {
        if (!dog.landing)
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
        if (stepClips.Length > 0)
        {
            int randomSource = Random.Range(0, stepClips.Length);
            stepSource.volume = Random.Range(minVolume, maxVolume);
            stepSource.pitch = Random.Range(minPitch, maxPitch);
            stepSource.PlayOneShot(stepClips[randomSource]);
        }
    }


}
