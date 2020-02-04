using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOrLever : MonoBehaviour
{
    public string incactiveAnimationName, activeAnimationName;
    public bool cannotBeTurnedOffAgain;

    [HideInInspector]
    public bool activated;

    AudioSource audioSource;
    Animator animator;

    //DELETE THIS WHEN WE HAVE ANIMATIONS
    Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //animator = GetComponent<Animator>();
        //animator.Play(incactiveAnimationName);

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
                GetComponent<SpriteRenderer>().color = startColor;//animator.Play(incactiveAnimationName);
                //Play sound

            }
            else
            {
                activated = true;
                GetComponent<SpriteRenderer>().color = Color.yellow;//animator.Play(activeAnimationName);
                //Play sound
            }
        }

    }
}
