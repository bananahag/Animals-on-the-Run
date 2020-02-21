using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyScared : MonkeyState
{
    [Tooltip("The sound effect that plays when the monkey is scared.")]
    public AudioClip scaredSFX;

    [Tooltip("The time (in seconds) the monkey stops before either curling up into a ball (if scared by a human) or runs away (if scared by water).")]
    public float scared = 0.5f;
    [Tooltip("The time (in seconds) the monkey runs away when scared by water.")]
    public float runAwayTime = 0.5f;

    float timePassed, timePassed2;

    bool secondPhase;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        if (!monkey.scaredCheck)
        {
            timePassed = 0.0f;
            timePassed2 = 0.0f;
            secondPhase = false;
            monkey.rb2d.velocity = new Vector2(0.0f, 0.0f);
            monkey.rb2d.gravityScale = 0.0f;
            monkey.audioSource.PlayOneShot(scaredSFX);
            monkey.animator.Play("Placeholder Monkey Scared");
        }
        else
        {
            monkey.rb2d.velocity = new Vector2(0.0f, 0.0f);
            secondPhase = true;
        }

        monkey.jumpBuffer = false;
    }

    public override void Exit()
    {
        monkey.rb2d.gravityScale = monkey.startGravityScale;
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        if (scared < timePassed)
        {
            monkey.scaredCheck = true;
            if (monkey.grounded)
            {
                secondPhase = true;
            }
            else
                monkey.ChangeState(monkey.inAirState);

        }

        if (secondPhase)
            SecondPhaseOfBeingScared();
    }

    void SecondPhaseOfBeingScared()
    {
        if (monkey.scaryObject.transform.position.x >= monkey.transform.position.x)
            monkey.facingRight = false;
        else
            monkey.facingRight = true;

        if (monkey.runAwayScared)
        {
            if(monkey.carryingBucket)
                monkey.animator.Play("Placeholder Monkey Walk Bucket");
            else
                monkey.animator.Play("Placeholder Monkey Walk");

            if (monkey.facingRight)
            {
                if (monkey.carryingBucket)
                    monkey.rb2d.velocity = new Vector2(1 * monkey.groundedState.walkingSpeedWhenCarryingBucket, monkey.rb2d.velocity.y);
                else
                    monkey.rb2d.velocity = new Vector2(1 * monkey.groundedState.walkingSpeed, monkey.rb2d.velocity.y);
            }
            else
            {
                if (monkey.carryingBucket)
                    monkey.rb2d.velocity = new Vector2(-1 * monkey.groundedState.walkingSpeedWhenCarryingBucket, monkey.rb2d.velocity.y);
                else
                    monkey.rb2d.velocity = new Vector2(-1 * monkey.groundedState.walkingSpeed, monkey.rb2d.velocity.y);
            }
            timePassed2 += Time.deltaTime;
            if (runAwayTime < timePassed2)
            {
                monkey.scaredCheck = false;
                monkey.ChangeState(monkey.groundedState);
            }
        }
        else
        {
            monkey.animator.Play("Placeholder Monkey Crying");
        }
    }

    public override void FixedUpdate()
    {
        if (!secondPhase)
            monkey.movement = new Vector2(0.0f, 0.0f);
        else
            monkey.movement = new Vector2(monkey.rb2d.velocity.x, monkey.rb2d.velocity.y);
    }
}
