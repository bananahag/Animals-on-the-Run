﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOrLever : MonoBehaviour
{
    public string incactiveAnimationName, activeAnimationName;
    public bool cannotBeTurnedOffAgain;
    [Tooltip("The sound effect that plays when the object is activated.")]
    public AudioClip activateSFX;
    [Tooltip("The elevator object that gets activated. Can be left null.")]
    public GameObject elevator = null;
    [Tooltip("The bridge object that gets activated. Can be left null.")]
    public GameObject bridge = null;
    [HideInInspector]
    public bool activated;

    AudioSource audioSource;
    Animator animator;

    bool noAnimator;

    //DELETE THIS WHEN WE HAVE ANIMATIONS
    Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
            animator.Play(incactiveAnimationName);
        }
        else
            noAnimator = true;

        startColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if (cannotBeTurnedOffAgain && activated) { }
        else
        {
            if (activated)
            {
                activated = false;
                if (elevator != null)
                    elevator.GetComponent<Elevator>().Activate(false, gameObject);
                if (bridge != null)
                    bridge.GetComponent<BridgeWheelmovement>().DraiSpakenKronk();
                if (noAnimator)
                    GetComponent<SpriteRenderer>().color = startColor;
                else
                    animator.Play(incactiveAnimationName);

            }
            else
            {
                activated = true;
                if(elevator != null)
                    elevator.GetComponent<Elevator>().Activate(true, gameObject);
                if (bridge != null)
                    bridge.GetComponent<BridgeWheelmovement>().DraiSpakenKronk();
                if (noAnimator)
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                else
                    animator.Play(activeAnimationName);
            }
            audioSource.PlayOneShot(activateSFX);
        }

    }
}


                