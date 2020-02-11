using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyInAir : MonkeyState
{
    [Tooltip("The speed of the monkey when she is in the air. (Only affects the horizontal speed)")]
    public float airSpeed = 5.0f;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void FixedUpdate()
    {
        if (monkey.x > 0)
            monkey.facingRight = true;
        else if (monkey.x < 0)
            monkey.facingRight = false;

        monkey.movement = new Vector2(monkey.x * airSpeed, monkey.rb2d.velocity.y);

        if (monkey.grounded && !monkey.jumping)
            monkey.ChangeState(monkey.landingState);

        if (monkey.rb2d.velocity.y != 0)
            monkey.landingVelocity = monkey.rb2d.velocity.y * -1;

        if (monkey.jumping)
            CheckIfJumping();

        AirAnimations();
    }

    void CheckIfJumping()
    {
        if (!Input.GetButton("Jump") && monkey.rb2d.velocity.y >= 0.0f)
        {
            monkey.movement = new Vector2(monkey.rb2d.velocity.x, monkey.rb2d.velocity.y / 2.0f);
            monkey.jumping = false;
        }

        if (monkey.rb2d.velocity.y < 0.0f)
            monkey.jumping = false;
    }

    void AirAnimations()
    {
        if (monkey.rb2d.velocity.y > 0.0f)
            monkey.animator.Play("Placeholder Monkey Jump");
        else if (monkey.rb2d.velocity.y < 0.0f)
            monkey.animator.Play("Placeholder Monkey Fall");
    }
}
