using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyInAir : MonkeyState
{
    [Tooltip("The speed of the monkey when she is in the air. (Only affects the horizontal speed)")]
    public float airSpeed = 4.0f;
    [Tooltip("The duration of time (in seconds) where the monkey is unable to climb ladders after jumping. When the timer is up the monkey is able to climb again.")]
    public float cannotClimbAfterJumpingTimer = 0.25f;
    [Tooltip("The time (in seconds) you can press jump before landing to still jump when you land. Basically when you press jump a little bit too early the monkey still jumps. Please ask Albin if you're confused about what this means.")]
    public float jumpBufferDuration = 0.25f;

    bool canClimb;
    float timePassed = 0.0f, timePassed2 = 0.0f;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        if (monkey.eel != null && monkey.eel.GetComponent<Eel>() != null && monkey.carryingBucket)
            monkey.eel.GetComponent<Eel>().MonkeyInteraction(false);
        monkey.carryingBucket = false;
        timePassed = 0.0f;
        timePassed2 = 0.0f;
        monkey.jumpBuffer = false;

        if (monkey.jumping)
            canClimb = false;
        else
            canClimb = true;
    }

    public override void Update()
    {
        if (monkey.x > 0)
            monkey.facingRight = true;
        else if (monkey.x < 0)
            monkey.facingRight = false;

        AirAnimations();
        if (monkey.active)
            CheckInput();
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(monkey.x * airSpeed, monkey.rb2d.velocity.y);

        if (monkey.grounded && !monkey.jumping)
            monkey.ChangeState(monkey.landingState);

        if (monkey.rb2d.velocity.y != 0)
            monkey.landingVelocity = monkey.rb2d.velocity.y * -1;

        if (monkey.y != 0.0f && monkey.canClimb && canClimb && !monkey.grounded)
            monkey.ChangeState(monkey.climbingState);

        if (monkey.jumping)
            CheckIfJumping();

        if (!canClimb)
            CannotClimbAfterJumping();

        if (monkey.jumpBuffer)
            JumpBufferTimer();
        else
            timePassed2 = 0.0f;
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
            monkey.jumpBuffer = true;
    }

    void CannotClimbAfterJumping ()
    {
        timePassed += Time.deltaTime;
        if (cannotClimbAfterJumpingTimer < timePassed)
            canClimb = true;
    }

    void JumpBufferTimer()
    {
        timePassed2 += Time.deltaTime;
        if (jumpBufferDuration < timePassed2)
            monkey.jumpBuffer = false;
    }

    void CheckIfJumping()
    {
        if (!Input.GetButton("Jump") && monkey.rb2d.velocity.y >= 0.0f || !monkey.active)
        {
            monkey.movement = new Vector2(monkey.rb2d.velocity.x, monkey.rb2d.velocity.y / 2.0f);
            monkey.jumping = false;
        }

        if (monkey.rb2d.velocity.y < 0.0f)
            monkey.jumping = false;
    }

    void AirAnimations()
    {
        if (!monkey.carryingBucket)
        {
            if (monkey.rb2d.velocity.y > 0.0f)
                monkey.animator.Play("Placeholder Monkey Jump");
            else if (monkey.rb2d.velocity.y < 0.0f)
                monkey.animator.Play("Placeholder Monkey Fall");
        }
        else
            monkey.animator.Play("Placeholder Monkey Fall");
    }
}
