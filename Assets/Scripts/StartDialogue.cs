﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [Tooltip("If true the text will only be displayed once")]
    public bool onlyActivatesOnce = true;
    [Tooltip("If true the text will start as soon as a character enters the collider, if not the player will have to press the interact button first")]
    public bool dialogueStartsWhenEntering = true;

    bool touching;
    bool hasPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (touching && Input.GetButtonDown("Interact") && !DialogueManager.isOpen && DialogueManager.canContinue)
        {
            if (onlyActivatesOnce && !hasPlayed)
                GetComponent<DialogueTrigger>().TriggerDialogue();
            else if (!onlyActivatesOnce)
                GetComponent<DialogueTrigger>().TriggerDialogue();
            hasPlayed = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (dialogueStartsWhenEntering && !DialogueManager.isOpen && DialogueManager.canContinue)
            {
                if (onlyActivatesOnce && !hasPlayed)
                    GetComponent<DialogueTrigger>().TriggerDialogue();
                else if (!onlyActivatesOnce)
                    GetComponent<DialogueTrigger>().TriggerDialogue();
                hasPlayed = true;
            }
            else
                touching = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            touching = false;
    }
}
