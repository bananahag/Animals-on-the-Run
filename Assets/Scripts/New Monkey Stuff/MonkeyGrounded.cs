using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyGrounded : MonkeyState
{
    [Tooltip("Sound effect that plays when the monkey takes a step on the ground.")]
    public AudioClip stepSFX;

    [Tooltip("The walking speed of the monkey when she IS NOT carrying the bucket.")]
    public float walkingSpeed = 4.0f;
    [Tooltip("The walking speed of the monkey when she IS carrying the bucket.")]
    public float walkingSpeedWhenCarryingBucket = 3.0f;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Update()
    {
        if (monkey.active)
            CheckInput();
        if (monkey.x > 0)
            monkey.facingRight = true;
        else if (monkey.x < 0)
            monkey.facingRight = false;

        GroundedAnimations();
    }

    public override void FixedUpdate()
    {
        if (monkey.carryingBucket)
            monkey.movement = new Vector2(monkey.x * walkingSpeedWhenCarryingBucket, monkey.rb2d.velocity.y);
        else
            monkey.movement = new Vector2(monkey.x * walkingSpeed, monkey.rb2d.velocity.y);

        if (monkey.y > 0.0f && monkey.canClimb && !monkey.carryingBucket)
            monkey.ChangeState(monkey.climbingState);

        
        CheckIfFalling();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump") || monkey.jumpBuffer)
            monkey.ChangeState(monkey.jumpsquatState);

        if (Input.GetButtonDown("Interact"))
        {
            if (monkey.canPickUpEel && !monkey.carryingBucket)
                monkey.ChangeState(monkey.pickingUpState);
            else if (monkey.carryingBucket)
                monkey.ChangeState(monkey.puttingDownState);

            else if (monkey.canOpenCage)
            {
                monkey.cage.GetComponent<Cage>().Open();
                monkey.ChangeState(monkey.interactState);
                monkey.canOpenCage = false;
            }
            
            else if (monkey.canPullLever)
            {
                monkey.lever.GetComponent<ButtonOrLever>().Activate();
                monkey.ChangeState(monkey.interactState);
            }
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
        if (!monkey.grounded)
            monkey.ChangeState(monkey.inAirState);
    }

    public void PlayStepSound()
    {
        monkey.audioSource.PlayOneShot(stepSFX);
    }
}
