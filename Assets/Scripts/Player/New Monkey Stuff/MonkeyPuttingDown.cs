using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyPuttingDown : MonkeyState
{
    [Tooltip("Audio source that plays when putting down the eel.")]
    public AudioSource putDownSource;

    [Tooltip("The time (in seconds) the monkey stops when putting down the eel.")]
    public float putDownTime = 1.0f;

    float timePassed;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        putDownSource.Play();
        monkey.animator.Play("Placeholder Monkey Pickdown");
       
    }

    public override void Exit()
    {
        monkey.carryingBucket = false;
        monkey.playLightAnim = false;
        if (monkey.eel.GetComponent<Eel>() != null)
            monkey.eel.GetComponent<Eel>().MonkeyInteraction(false);
    }

    public override void Update()
    {
        monkey.animator.Play("Placeholder Monkey Pickdown");
        timePassed += Time.deltaTime;
        if (putDownTime < timePassed)
        {
            monkey.ChangeState(monkey.groundedState);
        }
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.rb2d.velocity.y);
    }
}
