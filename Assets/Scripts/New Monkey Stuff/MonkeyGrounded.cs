using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyGrounded : MonkeyState
{
    [Tooltip("Sound effect that plays when the monkey takes a step on the ground.")]
    public AudioClip stepSFX;

    [Tooltip("The walking speed of the monkey when she IS NOT carrying the bucket.")]
    public float walkingSpeed = 5.0f;
    [Tooltip("The walking speed of the monkey when she IS carrying the bucket.")]
    public float walkingSpeedWhenCarryingBucket = 3.5f;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Update()
    {
        CheckInput();
    }

    public override void FixedUpdate()
    {
        if (monkey.x > 0)
            monkey.facingRight = true;
        else if (monkey.x < 0)
            monkey.facingRight = false;

        if (monkey.carryingBucket)
            monkey.movement = new Vector2(monkey.x * walkingSpeedWhenCarryingBucket, monkey.rb2d.velocity.y);
        else
            monkey.movement = new Vector2(monkey.x * walkingSpeed, monkey.rb2d.velocity.y);

        GroundedAnimations();
        CheckIfFalling();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            monkey.ChangeState(monkey.jumpsquatState);
        }
    }

    void GroundedAnimations()
    {
        if (monkey.x != 0)
        {
            if (monkey.carryingBucket)
                monkey.animator.Play("Placeholder Monkey Walk Bucket");
            else
                monkey.animator.Play("Placeholder Monkey Walk");
        }
        else
        {
            if (monkey.carryingBucket)
                monkey.animator.Play("Placeholder Monkey Idle Bucket");
            else
                monkey.animator.Play("Placeholder Monkey Idle");
        }
    }

    void CheckIfFalling()
    {
        //if (!monkey.grounded)
            //Debug.Log("I'm not on the ground! Oh dang!");//monkey.ChangeState(monkey.inAirState);
    }

    public void PlayStepSound()
    {
        monkey.audioSource.PlayOneShot(stepSFX);
    }
}
