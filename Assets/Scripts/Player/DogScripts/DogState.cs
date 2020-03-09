using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class DogState
{
    [HideInInspector]
    public DogBehaviour dog = null;

    public virtual void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public virtual void Enter() {

    }

    public virtual void Exit() {

    }

    public virtual void Update() {

    }

    public virtual void FixedUpdate() {

    }

    public virtual void OnTriggerEnter2D(Collider2D other) {

    }

    public virtual void OnTriggerExit2D(Collider2D other) {

    }
}
