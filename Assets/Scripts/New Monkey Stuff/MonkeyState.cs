using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class MonkeyState
{
    [HideInInspector]
    public MonkeyBehavior monkey = null;

    public virtual void OnValidate(MonkeyBehavior monkey)
    {
        this.monkey = monkey;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
