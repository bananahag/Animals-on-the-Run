using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyJumpsquat : MonkeyState
{
    [Tooltip("Audio source that plays when the monkey jumps.")]
    public AudioSource jumpSource;
    [Tooltip("Time (in seconds) spend crouching before the actual jumping part of the jump starts.")]
    public float jumpSquatTime = 0.125f;
    [Tooltip("The y velocity the monkey when the jump starts. Basically just means ''jump height.''")]
    public float jumpVelocity = 10.0f;

    float timePassed;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        monkey.rb2d.gravityScale = 0.0f;
        monkey.animator.Play("Placeholder Monkey Jumpsquat");
    }

    public override void Exit()
    {
        monkey.jumping = true;
        monkey.rb2d.gravityScale = monkey.startGravityScale;
        monkey.rb2d.velocity = new Vector2(monkey.rb2d.velocity.x, jumpVelocity);
        jumpSource.Play();
        monkey.animator.Play("Placeholder Monkey Jump");
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        if (jumpSquatTime < timePassed)
        {
            monkey.ChangeState(monkey.inAirState);
        }
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.rb2d.velocity.y);
    }
}
