using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyPickingUp : MonkeyState
{
    [Tooltip("Audio source that plays when picking up the eel.")]
    public AudioSource pickingUpSource;

    [Tooltip("The time (in seconds) the monkey stops when picking up the eel.")]
    public float pickUpTime = 1.0f;

    float timePassed;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        pickingUpSource.Play();
        monkey.animator.Play("Placeholder Monkey Pickup");
        
    }

    public override void Exit()
    {
        monkey.carryingBucket = true;
    }

    public override void Update()
    {
        monkey.animator.Play("Placeholder Monkey Pickup");
        timePassed += Time.deltaTime;
        if (pickUpTime < timePassed)
        {
            monkey.ChangeState(monkey.groundedState);
        }
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.rb2d.velocity.y);
    }
}
