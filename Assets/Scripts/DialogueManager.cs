using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    Queue<Dialogue.Animal> animals;
    Queue<AudioSource> customSources;
    Queue<string> sentences;
    static public bool isOpen, canContinue;
    [Tooltip("The audio sources that plays the different animal talking sound effects.")]
    public AudioSource monkeySource, dogSource, eelSource;
    [Tooltip("The text object that displays the dialogue text.")]
    public Text dialogueText;
    [Tooltip("The textbox object, basically.")]
    public Animator animator;
    [Tooltip("The images used for character portraits.")]
    public Image monkeyPortrait, dogPortrait, eelPortrait;
    [Tooltip("Time (in seconds) before the text shows up when the dialogue box pops up. If that doesn't make any sense, ask Albin.")]
    public float timeBeforeTextStarts = 0.1f;
    public enum DisplayType {TypeWriterStyle, AllTextAppearsAtOnce};
    public DisplayType displayType;

    enum ActiveCharacter {Monkey, Dog, Eel}
    ActiveCharacter activeCharacter;

    bool firstTextBox; //This is some real dumbo shit right here

    // Start is called before the first frame update
    void Start()
    {
        animals = new Queue<Dialogue.Animal>();
        customSources = new Queue<AudioSource>();
        sentences = new Queue<string>();
        canContinue = true;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (FindObjectOfType<MonkeyBehavior>() != null)
        {
            if (FindObjectOfType<MonkeyBehavior>().active)
                activeCharacter = ActiveCharacter.Monkey;
            FindObjectOfType<MonkeyBehavior>().active = false;
        }
        if (FindObjectOfType<DogBehaviour>() != null)
        {
            if (FindObjectOfType<DogBehaviour>().active)
                activeCharacter = ActiveCharacter.Dog;
            FindObjectOfType<DogBehaviour>().active = false;
        }
        if (FindObjectOfType<Eel>() != null)
        {
            if (FindObjectOfType<Eel>().active)
                activeCharacter = ActiveCharacter.Eel;
            FindObjectOfType<Eel>().active = false;
        }

        canContinue = false;
        firstTextBox = true;
        animals.Clear();
        monkeyPortrait.enabled = false;
        dogPortrait.enabled = false;
        eelPortrait.enabled = false;
        customSources.Clear();
        sentences.Clear();
        dialogueText.text = "";
        foreach (Dialogue.Animal animal in dialogue.animals)
        {
            animals.Enqueue(animal);
        }
        foreach (AudioSource customSource in dialogue.customSources)
        {
            customSources.Enqueue(customSource);
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
        {
            DisplayNextPortrait();
            StartCoroutine(StartUp());
        }
        animator.SetBool("isOpen", true);
        isOpen = true;
    }

    public void DisplayNextSentence()
    {
        if (canContinue)
        {
            if(sentences.Count == 0)
            {
                StartCoroutine(EndDialogue());
                return;
            }
            if (firstTextBox)
                firstTextBox = false;
            else
                DisplayNextPortrait();
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    void DisplayNextPortrait()
    {
        Dialogue.Animal animal = animals.Dequeue();
        AudioSource customSource = customSources.Dequeue();
        if (animal == Dialogue.Animal.Monkey)
        {
            if (customSource == null)
                monkeySource.Play();
            else
                customSource.Play();

            monkeyPortrait.enabled = true;
        }
        else { monkeyPortrait.enabled = false; }

        if (animal == Dialogue.Animal.Dog)
        {
            if (customSource == null)
                dogSource.Play();
            else
                customSource.Play();
            dogPortrait.enabled = true;
        }
        else { dogPortrait.enabled = false; }

        if (animal == Dialogue.Animal.Eel)
        {
            if (customSource == null)
                eelSource.Play();
            else
                customSource.Play();
            eelPortrait.enabled = true;
        }
        else { eelPortrait.enabled = false; }
    }

    IEnumerator EndDialogue()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);

        yield return null;

        if (activeCharacter == ActiveCharacter.Monkey && FindObjectOfType<MonkeyBehavior>() != null)
            FindObjectOfType<MonkeyBehavior>().active = true;
        else if (activeCharacter == ActiveCharacter.Dog && FindObjectOfType<DogBehaviour>() != null)
            FindObjectOfType<DogBehaviour>().active = true;
        else if (activeCharacter == ActiveCharacter.Eel && FindObjectOfType<Eel>() != null)
            FindObjectOfType<Eel>().active = true;
    }

    IEnumerator StartUp()
    {
        yield return new WaitForSeconds(timeBeforeTextStarts);
        canContinue = true;
        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (displayType == DisplayType.TypeWriterStyle)
        {
            int fastText = 0;
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                canContinue = false;
                dialogueText.text += letter;
                fastText++;
                if (fastText > 1)
                {
                    yield return new WaitForFixedUpdate();
                    fastText = 0;
                }
            }
        }
        else if (displayType == DisplayType.AllTextAppearsAtOnce)
        {
            float changeTextColor = 0.0f;
            dialogueText.text = sentence;
            for (int i = 0; i < 5; i++)
            {
                canContinue = false;
                dialogueText.color = new Color(changeTextColor, changeTextColor, changeTextColor, 1.0f);
                yield return new WaitForFixedUpdate();
                changeTextColor += 0.25f;
                if (changeTextColor > 1.0f)
                    changeTextColor = 1.0f;
            }
        }
        canContinue = true;
    }
}
