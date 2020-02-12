using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyClimbing : MonkeyState
{

    [Tooltip("The distance between the center of the monkey and the center of the ladder when climbing.")]
    public float ladderCenterOffsetDistance = 0.25f;
    [Tooltip("The vertical climbing speed of the monkey.")]
    public float climbingSpeed = 4.0f;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);//CAN CLIMB AFTER JUMPING THING
    }

    public override void Enter()
    {
        monkey.animator.Play("Placeholder Monkey Climb");
        monkey.rb2d.gravityScale = 0.0f;
        monkey.movement = new Vector2(0.0f, 0.0f);
        if (monkey.transform.position.x <= monkey.ladderXPosition && monkey.oneWayLadder == 0 || monkey.oneWayLadder == 2)
        {
            monkey.transform.position = new Vector3(monkey.ladderXPosition - ladderCenterOffsetDistance, monkey.transform.position.y, monkey.transform.position.z);
            monkey.facingRight = true;
            monkey.spriteRenderer.flipX = false;

        }
        else
        {
            monkey.transform.position = new Vector3(monkey.ladderXPosition + ladderCenterOffsetDistance, monkey.transform.position.y, monkey.transform.position.z);
            monkey.facingRight = false;
            monkey.spriteRenderer.flipX = true;
        }
    }

    public override void Exit()
    {
        monkey.rb2d.gravityScale = monkey.startGravityScale;
        monkey.movement = new Vector2(monkey.rb2d.velocity.x, monkey.rb2d.velocity.y);
        //monkey.canChangeClimbingDirection = false;
        monkey.animator.enabled = true;
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.y * climbingSpeed);
        if (!monkey.canClimb ||monkey.grounded && monkey.y < 0.0f)
            monkey.ChangeState(monkey.inAirState);

        if (monkey.rb2d.velocity.y != 0)
            monkey.landingVelocity = monkey.rb2d.velocity.y * -1;

        Climbing();
    }

    void Climbing()
    {
        /*
        if (monkey.y != 0)
            monkey.animator.enabled = true;
        else
            monkey.animator.enabled = false;
        if (monkey.x != 0.0f && canChangeClimbingDirection && monkey.oneWayLadder == 0)
        {
            if (monkey.x > 0.0f)
            {
                monkey.transform.position = new Vector3(monkey.ladderXPosition + ladderCenterOffsetDistance, monkey.transform.position.y, monkey.transform.position.z);
                monkey.facingRight = false;
                monkey.spriteRenderer.flipX = true;
            }
            else if (monkey.x < 0.0f)
            {
                monkey.transform.position = new Vector3(monkey.ladderXPosition - ladderCenterOffsetDistance, monkey.transform.position.y, monkey.transform.position.z);
                monkey.facingRight = true;
                monkey.spriteRenderer.flipX = false;
            }

            if (monkey.y == 0)
            {

                StartCoroutine(StopClimbing());
            }
        }

        if (monkey.oneWayLadder != 0 && monkey.y == 0)
        {
            if (monkey.oneWayLadder == 1 && monkey.x > 0.0f)
                StartCoroutine(StopClimbing());
            else if (monkey.oneWayLadder == 2 && monkey.x < 0.0f)
                StartCoroutine(StopClimbing());
        }
        if (monkey.x == 0.0f)
            canChangeClimbingDirection = true;

        if (monkey.y != 0.0f && canPlayClimbingSoundAgain)
        {
            StartCoroutine(ClimbingSoundsLoop());
            canPlayClimbingSoundAgain = false;
        }
        */
    }
}
