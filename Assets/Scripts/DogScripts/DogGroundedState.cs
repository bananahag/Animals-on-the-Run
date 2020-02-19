using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogGroundedState : DogState
{
    public AudioClip stepSFX;
    public float walkingSpeed = 4.0f;

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
            dog.facingRight = true;
        else if (dog.x < 0)
            dog.facingRight = false;

        GroundedAnimations();

        if (!dog.grounded)
        {
            dog.ChangeState(dog.inAirState);
        }
    }

    public override void FixedUpdate()
    {
        dog.movement = new Vector2(dog.x * walkingSpeed, dog.rb2d.velocity.y);

        CheckIfFalling();
    }

    public void CheckInput()
    {
        if (Input.GetButtonDown("Jump") || dog.jumpBuffer)
            dog.ChangeState(dog.jumpsquatState);
        //Interacts, Charm & movable objects.
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject") && Input.GetKeyDown("Input"))
        {
            dog.ChangeState(dog.pushingState);
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {

    }

    public void GroundedAnimations()
    {
        if (dog.movement.x != 0)
        {
            dog.animator.Play("Walking");
        }
        if (dog.movement.x == 0)
        {
            dog.animator.Play("Idle");
        }
    }

    void CheckIfFalling()
    {
        if (!dog.grounded)
            dog.ChangeState(dog.inAirState);
    }
    public void PlayStepSound()
    {
        dog.audioSource.PlayOneShot(stepSFX);
    }


}
