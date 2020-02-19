using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogCharmingState : DogState
{
    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void OnTriggerEnter2D(Collider2D other)
    {

    }

    public override void OnTriggerExit2D(Collider2D other)
    {

    }

}
