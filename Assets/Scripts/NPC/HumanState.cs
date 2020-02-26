using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanState 
{
    [HideInInspector]
    public HumanBehavior human = null;


    public virtual void OnValidate(HumanBehavior human)
    {
        this.human = human;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }


}
