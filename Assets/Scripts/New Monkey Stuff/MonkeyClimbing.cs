using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyClimbing : MonkeyState
{
    [Tooltip("The sound effect that plays when the monkey is climbing.")]
    public AudioClip ladderClimbSFX;

    [Tooltip("The distance between the center of the monkey and the center of the ladder when climbing.")]
    public float ladderCenterOffsetDistance = 0.25f;
    [Tooltip("The vertical climbing speed of the monkey.")]
    public float climbingSpeed = 4.0f;
    [Tooltip("Time (in seconds) between each sound effect when climbing.")]
    public float timeBetweenClimbingSounds = 0.25f;
    [Tooltip("Time (in seconds) before the monkey lets go of the ladder when holding left or right.")]
    public float stopClimbingTime = 0.5f;
    [Tooltip("Time (in seconds) before the monkey can change the direction she's facing before you let go of both the left and right buttons. (This is really hard to explain, so if you want something clarified please ask Albin so he can try to explain this better in person)")]
    public float canChangeClimbingDirectionTime = 0.5f;

    bool canChangeClimbingDirection, canPlayClimbingSoundAgain;
    float timePassed1, timePassed2, timePassed3;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);//CAN CLIMB AFTER JUMPING THING
    }

    public override void Enter()
    {
        monkey.animator.Play("Placeholder Monkey Climb");
        monkey.rb2d.gravityScale = 0.0f;
        monkey.movement = new Vector2(0.0f, 0.0f);
        canChangeClimbingDirection = false;
        canPlayClimbingSoundAgain = true;
        timePassed1 = 0.0f;
        timePassed2 = 0.0f;
        timePassed3 = 0.0f;

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
        monkey.animator.enabled = true;
    }

    public override void Update()
    {
        ClimbingSoundsLoop();
        CheckInput();
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.y * climbingSpeed);
        if (!monkey.canClimb)
        {
            Debug.Log("Test");
            monkey.ChangeState(monkey.inAirState);
        }
        if (monkey.grounded && monkey.y < 0.0f)
            monkey.ChangeState(monkey.inAirState);

        if (monkey.rb2d.velocity.y != 0)
            monkey.landingVelocity = monkey.rb2d.velocity.y * -1;

        Climbing();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            monkey.audioSource.PlayOneShot(ladderClimbSFX);
            monkey.ChangeState(monkey.jumpsquatState);
            monkey.rb2d.velocity = new Vector2(0.0f, 0.0f);
        }
    }

    void Climbing()
    {
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
                StopClimbing();
            else
                timePassed2 = 0.0f;
        }

        if (monkey.oneWayLadder != 0 && monkey.y == 0)
        {
            if (monkey.oneWayLadder == 1 && monkey.x > 0.0f)
                StopClimbing();
            else if (monkey.oneWayLadder == 2 && monkey.x < 0.0f)
                StopClimbing();
        }
        if (monkey.x == 0.0f)
        {
            canChangeClimbingDirection = true;
            timePassed2 = 0.0f;
        }

        if (monkey.y != 0.0f && canPlayClimbingSoundAgain)
        {
            monkey.audioSource.PlayOneShot(ladderClimbSFX);
            timePassed3 = 0.0f;
            canPlayClimbingSoundAgain = false;
        }

        if (!canChangeClimbingDirection)
            CanChangeClimbingDirectionTimer();

    }

    void CanChangeClimbingDirectionTimer()
    {
        timePassed1 += Time.deltaTime;
        if (canChangeClimbingDirectionTime < timePassed1)
        {
            canChangeClimbingDirection = true;
        }
    }

    void StopClimbing()
    {
        timePassed2 += Time.deltaTime;
        bool holdingRight;
        if (monkey.x > 0.0f)
            holdingRight = true;
        else
            holdingRight = false;

        if (stopClimbingTime < timePassed2)
        {
            if (holdingRight && monkey.x > 0.0f && monkey.y == 0 || !holdingRight && monkey.x < 0.0f && monkey.y == 0)
                monkey.ChangeState(monkey.inAirState);
        }
    }

    void ClimbingSoundsLoop()
    {
        if (!canPlayClimbingSoundAgain)
        {
            timePassed3 += Time.deltaTime;
            if (timeBetweenClimbingSounds < timePassed3)
            {
                if (monkey.y != 0.0f)
                {
                    monkey.audioSource.PlayOneShot(ladderClimbSFX);
                    timePassed3 = 0.0f;
                }
                else
                    canPlayClimbingSoundAgain = true;
            }
        }
    }
}
