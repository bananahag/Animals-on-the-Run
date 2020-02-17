﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyLanding : MonkeyState
{
    [Tooltip("Sound effect that plays when the monkey lands.")]
    public AudioClip landingSFX;
    [Tooltip("Maximum amount of time (in seconds) that the monkey will be stopped when landing after a great fall.")]
    public float maxLandingTime = 0.2f;
    [Tooltip("Number that gets multiplied with the falling velocity of the monkey to calculate the time spent waiting when landing.")]
    public float landingTimeMultiplier = 0.015f;

    float timePassed;
    float landingTime;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        monkey.rb2d.gravityScale = 0.0f;
        monkey.animator.Play("Placeholder Monkey Land");
        monkey.audioSource.PlayOneShot(landingSFX);

        landingTime = landingTimeMultiplier * monkey.landingVelocity;

        if (landingTime > maxLandingTime)
            landingTime = maxLandingTime;
    }

    public override void Exit()
    {
        monkey.rb2d.gravityScale = monkey.startGravityScale;
    }

    public override void Update()
    {
        timePassed += Time.deltaTime;
        if (landingTime < timePassed)
        {
            if (monkey.scaredCheck)
                monkey.ChangeState(monkey.scaredState);
            else if (monkey.jumpBuffer)
                monkey.ChangeState(monkey.jumpsquatState);
            else
                monkey.ChangeState(monkey.groundedState);
        }

        if (monkey.active && !monkey.scaredCheck)
            CheckInput();
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.rb2d.velocity.y);
        if (monkey.y > 0.0f && monkey.canClimb)
            monkey.ChangeState(monkey.climbingState);
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
            monkey.jumpBuffer = true;
    }
}
