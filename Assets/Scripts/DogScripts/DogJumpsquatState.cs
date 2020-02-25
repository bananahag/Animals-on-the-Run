using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogJumpsquatState : DogState
{
    [Tooltip("Audio source that plays when the dog jumps.")]
    public AudioSource jumpSource;
    [Tooltip("Time (in seconds) spend crouching before the actual jumping part of the jump starts.")]
    public float jumpSquatTime = 0.125f;
    [Tooltip("The y velocity of the dog when the jump starts. Basically just means ''jump height.''")]
    public float jumpVelocity = 10.0f;

    float timePassed;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        dog.rb2d.gravityScale = 0.0f;
        dog.animator.Play("Jumpsquat");
    }

    public override void Exit()
    {
        dog.jumping = true;
        dog.rb2d.gravityScale = dog.startGravityScale;
        dog.rb2d.velocity = new Vector2(dog.rb2d.velocity.x, jumpVelocity);
        jumpSource.Play();
        dog.animator.Play("Jump");
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        if (jumpSquatTime < timePassed)
        {
            dog.ChangeState(dog.inAirState);
        }
    }

    public override void FixedUpdate()
    {
        dog.movement = new Vector2(0.0f, dog.rb2d.velocity.y);
    }
}
