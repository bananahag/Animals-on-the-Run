using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDialogue : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && DialogueManager.isOpen && DialogueManager.canContinue)
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }
}
