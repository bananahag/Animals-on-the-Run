using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogInAirState : DogState
{
    float timePassed = 0.0f;
    [Tooltip("The time (in seconds) you can press jump before landing to still jump when you land. Basically when you press jump a little bit too early the dog still jumps. Please ask Albin if you're confused about what this means.")]
    public float jumpBufferDuration = 0.25f;
    public float airSpeed = 4.0f;

    public AudioClip landingSFX;
    public AudioClip jumpSFX;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        dog.jumpBuffer = false;
        Debug.Log("Dog In Air State");
        dog.audioSource.PlayOneShot(jumpSFX);
    }

    public override void Exit()
    {
        dog.audioSource.PlayOneShot(landingSFX);
    }

    public override void Update()
    {
        AirAnimations();

        if (dog.active)
        {
            CheckInput();
        }

        if (dog.x > 0)
        {
            dog.facingRight = true;
        } else if (dog.x < 0)
        {
            dog.facingRight = false;
        }
    }

    public override void FixedUpdate()
    {

        dog.movement = new Vector2(dog.x * airSpeed, dog.rb2d.velocity.y);

        if (dog.grounded && !dog.jumping && !dog.movingObject)
        {
            dog.ChangeState(dog.groundedState);
        }
        if (dog.rb2d.velocity.y != 0)
        {
            dog.landingVelocity = dog.rb2d.velocity.y * -2;
        }
        if (dog.jumping)
        {
            CheckIfJumping();
        }
        if (dog.jumpBuffer) 
        {
            JumpBufferTimer();
        }       
        else
        {
            timePassed = 0.0f;
        }
    }

    void JumpBufferTimer()
    {
        timePassed += Time.deltaTime;
        if (jumpBufferDuration < timePassed)
            dog.jumpBuffer = false;
    }

    public void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
            dog.jumpBuffer = true;
    }

    void CheckIfJumping()
    {
        if (!Input.GetButton("Jump") && dog.rb2d.velocity.y >= 0.0f || !dog.active)
        {
            dog.movement = new Vector2(dog.rb2d.velocity.x, dog.rb2d.velocity.y / 6.0f);
            dog.jumping = false;
        }

        if (dog.rb2d.velocity.y < 0.0f)
            dog.jumping = false;
    }

    public void AirAnimations()
    {
        if (dog.movement.y < 0)
        {
            dog.animator.Play("Falling");
        }
        else if (dog.movement.y >= 0)
        {
            dog.animator.Play("Jumping");
        }
    }
}
