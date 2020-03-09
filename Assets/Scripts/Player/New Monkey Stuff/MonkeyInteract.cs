using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyInteract : MonkeyState
{
    [Tooltip("Audio sources that plays when interacting with an object.")]
    public AudioSource[] interactSources;

    [Tooltip("The time (in seconds) the monkey stops when interacting with an object.")]
    public float interactTime = 1.0f;

    float timePassed;

    public override void OnValidate(MonkeyBehavior monkey)
    {
        base.OnValidate(monkey);
    }

    public override void Enter()
    {
        timePassed = 0.0f;
        int randomSource = Random.Range(0, interactSources.Length);
        interactSources[randomSource].Play();
        monkey.animator.Play("Placeholder Monkey Interact");
    }

    public override void Update()
    {
        monkey.animator.Play("Placeholder Monkey Interact");
        timePassed += Time.deltaTime;
        if (interactTime < timePassed)
        {
            monkey.ChangeState(monkey.groundedState);
        }
    }

    public override void FixedUpdate()
    {
        monkey.movement = new Vector2(0.0f, monkey.rb2d.velocity.y);
    }
}
