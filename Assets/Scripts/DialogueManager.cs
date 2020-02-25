using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences;
    static public bool isOpen, canContinue;
    public Text dialogueText;
    public Animator animator;
    [Tooltip("Time (in seconds) before the text shows up when the dialogue box pops up. If that doesn't make any sense, ask Albin.")]
    public float timeBeforeTextStarts = 0.175f;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        canContinue = true;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        canContinue = false;
        sentences.Clear();
        dialogueText.text = "";
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        if (isOpen)
        {
            canContinue = true;
            DisplayNextSentence();
        }
        else
            StartCoroutine(StartUp());
        animator.SetBool("isOpen", true);
        isOpen = true;
    }

    public void DisplayNextSentence()
    {
        if (canContinue)
        {
            if(sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            string sentence = sentences.Dequeue();
            //if (sentence.Contains("Monkey:"))
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    void EndDialogue()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
    }

    IEnumerator StartUp()
    {
        yield return new WaitForSeconds(timeBeforeTextStarts);
        canContinue = true;
        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            canContinue = false;
            dialogueText.text += letter;
            yield return new WaitForFixedUpdate();
        }
        canContinue = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
