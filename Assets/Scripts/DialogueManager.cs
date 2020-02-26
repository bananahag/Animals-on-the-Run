using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    Queue<Dialogue.Animal> animals;
    Queue<string> sentences;
    static public bool isOpen, canContinue;
    [Tooltip("The text object that displays the dialogue text.")]
    public Text dialogueText;
    [Tooltip("The textbox object, basically.")]
    public Animator animator;
    [Tooltip("Time (in seconds) before the text shows up when the dialogue box pops up. If that doesn't make any sense, ask Albin.")]
    public float timeBeforeTextStarts = 0.175f;
    [Tooltip("The images used for character portraits.")]
    public Image monkeyPortrait, dogPortrait, eelPortrait;

    // Start is called before the first frame update
    void Start()
    {
        animals = new Queue<Dialogue.Animal>();
        sentences = new Queue<string>();
        canContinue = true;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        canContinue = false;
        animals.Clear();
        monkeyPortrait.enabled = false;
        dogPortrait.enabled = false;
        eelPortrait.enabled = false;
        sentences.Clear();
        dialogueText.text = "";
        foreach (Dialogue.Animal animal in System.Enum.GetValues(typeof(Dialogue.Animal)))
        {
            animals.Enqueue(animal);
        }
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
            Dialogue.Animal animal = animals.Dequeue();
            if (animal == Dialogue.Animal.Monkey)
                monkeyPortrait.enabled = true;
            else { monkeyPortrait.enabled = false; }

            if (animal == Dialogue.Animal.Dog)
                dogPortrait.enabled = true;
            else { dogPortrait.enabled = false; }

            if (animal == Dialogue.Animal.Eel)
                eelPortrait.enabled = true;
            else { eelPortrait.enabled = false; }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    void DisplayNextPortrait()
    {

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
