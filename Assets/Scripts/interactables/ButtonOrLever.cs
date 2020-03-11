using System.Collections;
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
    public bool activated, needsElectricity;



    SwapCharacter swapCharacter;

    AudioSource audioSource;
    Animator animator;

    bool noAnimator;

    //DELETE THIS WHEN WE HAVE ANIMATIONS
    Color startColor;

    // Start is called before the first frame update
    void Awake()
    {
        swapCharacter = FindObjectOfType<SwapCharacter>();
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

    public void Activate()
    {
        if (cannotBeTurnedOffAgain && activated) { }
        else
        {
            if (activated)
            {
                activated = false;
                if (elevator != null)
                {
                    if(swapCharacter != null)
                    {
                        swapCharacter.highlightedObject = elevator.gameObject;
                        swapCharacter.isElevator = true;
                    }
                    elevator.GetComponent<Elevator>().Activate(false, gameObject);
                }

                if (bridge != null)
                {
                    if (swapCharacter != null)
                    {
                        swapCharacter.highlightedObject = bridge.gameObject;
                        swapCharacter.isBridge = true;
                    }
                    bridge.GetComponent<BridgeWheelmovement>().DraiSpakenKronk();
                }

                if (noAnimator)
                    GetComponent<SpriteRenderer>().color = startColor;
                else
                    animator.Play(incactiveAnimationName);

            }
            else
            {
                activated = true;
                if(elevator != null)
                {
                    if (swapCharacter != null)
                    {
                        swapCharacter.highlightedObject = elevator.gameObject;
                        swapCharacter.isElevator = true;
                    }
                    elevator.GetComponent<Elevator>().Activate(true, gameObject);
                }

                if (bridge != null)
                {
                    if (swapCharacter != null)
                    {
                        swapCharacter.highlightedObject = bridge.gameObject;
                        swapCharacter.isBridge = true;
                    }
                    bridge.GetComponent<BridgeWheelmovement>().DraiSpakenKronk();
                }

                if (noAnimator)
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                else
                    animator.Play(activeAnimationName);
            }
            audioSource.PlayOneShot(activateSFX);
        }

    }

    public void Charge (bool charged)
    {
        needsElectricity = !charged;
        if (needsElectricity)
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        else
            GetComponent<SpriteRenderer>().color = startColor;
    }
}


                