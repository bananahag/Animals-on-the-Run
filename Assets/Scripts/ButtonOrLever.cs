using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOrLever : MonoBehaviour
{
    public string incactiveAnimationName, activeAnimationName;
    public bool cannotBeTurnedOffAgain;

    [HideInInspector]
    public bool activated;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(incactiveAnimationName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(bool turnOn)
    {
        if (cannotBeTurnedOffAgain && activated) { }
        else
            activated = turnOn;

        if (turnOn)
            print(this + " Has been activated");//animator.Play(activeAnimationName);
        else if (!cannotBeTurnedOffAgain)
            print(this + " Has been deactivated");//animator.Play(incactiveAnimationName);

    }
}
