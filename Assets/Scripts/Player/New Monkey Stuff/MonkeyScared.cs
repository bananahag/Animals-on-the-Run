using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyScared : MonkeyState
{
    [Tooltip("Audio source that plays when the monkey is scared.")]
    public AudioSource scaredSource;

    [Tooltip("The time (in seconds) the monkey stops before either curling up into a ball (if scared by a human) or runs away (if scared by water).")]
    public float scared = 0.5f;
    [Tooltip("The time (in seconds) the monkey runs away when scared by water.")]
    public float runAwayTime = 0.5f;

    float timePassed, timePassed2;
    private int notCharmed = 0;
    public float reactToHumanDistance = 10f;
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
            scaredSource.Play();
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
        monkey.scaredCheck = false;
    }

    public override void Update()
    {
        if (secondPhase)
            SecondPhaseOfBeingScared();
        notCharmed = 0;
        timePassed += Time.deltaTime;
        if (scared < timePassed)
        {
            monkey.scaredCheck = true;
            if (monkey.grounded)
            {
                secondPhase = true;
                
            }
            else
            {
                
                monkey.scaredCheck = false;
                monkey.ChangeState(monkey.inAirState);
            }

        }
        foreach (GameObject human in monkey.humansHit)
        {
            if (!human.GetComponentInParent<Human>().charmed)
            {
                notCharmed++;
            }
        }
        if (notCharmed == 0)
        {
            monkey.ChangeState(monkey.groundedState);
          
        }

        
           
    }

    void SecondPhaseOfBeingScared()
    {
        if (monkey.runAwayScared)
        {
            Debug.Log("Am here3");
            monkey.ChangeState(monkey.groundedState);
        }
        else
        {
            Debug.Log("Am here4");
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
