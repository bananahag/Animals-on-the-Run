using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogSwimmingState : DogState
{
    public float swimmingSpeed = 3.0f;
    public float swimJumpVelocity = 4.0f;
    public float swimJumpCoolDown = 0.25f;
    float timePassed = 0.0f;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        dog.swimming = true;
        dog.animator.Play("DogSwimming");
        dog.wet = true;
    }

    public override void Exit()
    {
        dog.WetTimer();
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
    }

    public void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            timePassed += Time.deltaTime;
            if(timePassed < swimJumpCoolDown)
            {
                dog.movement = new Vector2(dog.movement.x, dog.movement.y / 2.2f);
                dog.rb2d.velocity = new Vector2(dog.rb2d.velocity.x, swimJumpVelocity);
                timePassed = 0.0f;
            }
        }
    }

    public override void FixedUpdate()
    {
        dog.movement = new Vector2(dog.x * swimmingSpeed, dog.rb2d.velocity.y);
        if (!dog.swimming)
        {
            if (dog.grounded)
            {
                dog.ChangeState(dog.groundedState);
            }
            else if (!dog.grounded)
            {
                dog.ChangeState(dog.inAirState);
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            dog.swimming = false;
        }
    }

}
