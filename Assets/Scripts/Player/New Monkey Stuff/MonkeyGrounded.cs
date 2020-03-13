using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyGrounded : MonkeyState
{
    [Tooltip("Audio source that plays the audio clips.")]
    public AudioSource stepSource;
    [Tooltip("Audio clips that plays when the monkey takes a step on the ground.")]
    public AudioClip[] stepClips;
    [Tooltip("The minimum and the maximum volume of the footstep sounds.")]
    public float minVolume = 0.9f, maxVolume = 1.1f;
    [Tooltip("The minimum and the maximum pitch of the footstep sounds.")]
    public float minPitch = 0.9f, maxPitch = 1.1f;

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
        if (Input.GetButtonDown("Jump") && !monkey.carryingBucket || monkey.jumpBuffer && !monkey.carryingBucket)
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
                if (!monkey.lever.GetComponent<ButtonOrLever>().needsElectricity)
                {
                    monkey.lever.GetComponent<ButtonOrLever>().Activate();
                    monkey.ChangeState(monkey.interactState);
                }
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
        if (stepClips.Length > 0)
        {
            int randomSource = Random.Range(0, stepClips.Length);
            stepSource.volume = Random.Range(minVolume, maxVolume);
            stepSource.pitch = Random.Range(minPitch, maxPitch);
            stepSource.PlayOneShot(stepClips[randomSource]);
        }
    }
}
