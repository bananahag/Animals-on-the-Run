using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGroundedState : DogState
{

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
        CheckInput();
        GroundedAnimations();

        if (!dog.grounded)
        {
            dog.ChangeState(dog.inAirState);
        }
    }

    public override void FixedUpdate()
    {

    }

    public void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            dog.ChangeState(dog.inAirState);
        }
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
            dog.animator.Play("DogRunning");
        }
        if (dog.movement.x == 0)
        {
            dog.animator.Play("DogIdle");
        }
    }

}
