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

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        CheckInput();
    }

    public override void FixedUpdate()
    {

    }

    public void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            dog.ChangeState(dog.jumpingState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {

    }

    public override void OnTriggerExit2D(Collider2D other)
    {

    }

}
