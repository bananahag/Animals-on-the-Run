﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCharacter : MonoBehaviour
{
    public Image monkeyImage, dogImage, eelImage;

    public bool keepMusicWhenChangingScene = false;
    static public bool keepMusic = true;

    MenyUI mMeny;
    public List<GameObject> characters;
    private MonkeyBehavior mMonkey;
    private DogBehaviour mDog;
    private Eel mEel;
    //[HideInInspector]
    public GameObject highlightedObject;
    [HideInInspector]
    public bool isHighlighting;
    [HideInInspector]
    public bool isElevator;
    [HideInInspector]
    public bool isBridge;
    [HideInInspector]
    public bool hasBeenActivated = false;

    public float cameraYOffset = 3.0f;

    public int SelectedChar;
    private Camera cam;
    Vector3 oldCameraPos;

    public float runToTheRightTime = 5.0f;


    public float cameraTravelTime = 1.0f;
    public float objectTravelTimeMultiplier = 0.5f;
    float timePassed;
    float fraction;
    int targetPosition;

    bool animalsRunning;

    public Vector2 minCameraPos = new Vector2(-100, -100), maxCameraPos = new Vector2(100, 100);

    public enum ActiveCharacter
    {
        Monkey,
        Dog,
        Eel,
    }

    private void Awake()
    {
        keepMusic = true;
        isHighlighting = false;
        mMonkey = characters[0].GetComponent<MonkeyBehavior>();
        mDog = characters[1].GetComponent<DogBehaviour>();
        mEel = characters[2].GetComponent<Eel>();
        timePassed = 0.0f;
        fraction = 1.0f;
    }
    void Start()
    {
        animalsRunning = false;
        SelectedChar = 0;   
        cam = Camera.main;
        mMeny = FindObjectOfType<MenyUI>().GetComponent<MenyUI>();
        oldCameraPos = cam.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(highlightedObject != null)
        {
            if (isBridge)
            {
                if (!highlightedObject.GetComponent<BridgeWheelmovement>().highlighted)
                {
                    isHighlighting = true;
                }
            }
            else if (isElevator)
            {
                if (!highlightedObject.GetComponent<Elevator>().highlighted)
                {
                    isHighlighting = true;
                }
            }
        }

        if (!isHighlighting)
        {
            if (Input.GetButtonDown("Swap1") && !DialogueManager.isOpen && DialogueManager.canContinue && !animalsRunning)
            {
                timePassed = 0.0f;
                fraction = 0.0f;
                oldCameraPos = cam.transform.position;
                if (SelectedChar < characters.Count - 1)
                {
                    SelectedChar++;
                }
                else
                {
                    SelectedChar = 0;
                }
                if (SelectedChar == (int)ActiveCharacter.Dog)
                {
                    characters[0].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[0].GetComponent<Rigidbody2D>().velocity.y, 0);
                }
                else if (SelectedChar == (int)ActiveCharacter.Monkey)
                {
                    characters[1].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[1].GetComponent<Rigidbody2D>().velocity.y, 0);
                }
            }
            else if (Input.GetButtonDown("Swap2") && !DialogueManager.isOpen && DialogueManager.canContinue && !animalsRunning)
            {
                timePassed = 0.0f;
                fraction = 0.0f;
                oldCameraPos = cam.transform.position;
                if (SelectedChar > 0)
                {
                    SelectedChar--;
                }
                else
                {
                    SelectedChar = characters.Count - 1;
                }

                if (SelectedChar == (int)ActiveCharacter.Dog)
                {
                    characters[0].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[0].GetComponent<Rigidbody2D>().velocity.y, 0);
                }
                else if (SelectedChar == (int)ActiveCharacter.Monkey)
                {
                    characters[1].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[1].GetComponent<Rigidbody2D>().velocity.y, 0);
                }
            }

            if (SelectedChar == (int)ActiveCharacter.Monkey)
            {
                if (!DialogueManager.isOpen && DialogueManager.canContinue && !animalsRunning)
                {
                    mEel.active = false;
                    mDog.active = false;
                    mMonkey.active = true;
                }
                
                if (monkeyImage != null && dogImage != null && eelImage != null)
                {
                    monkeyImage.rectTransform.sizeDelta = new Vector2(75.0f, 75.0f);
                    dogImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                    eelImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                }
                if (fraction >= 1.0f)
                    cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y + cameraYOffset, cam.transform.position.z);
            }

            else if (SelectedChar == (int)ActiveCharacter.Dog)
            {
                if (!DialogueManager.isOpen && DialogueManager.canContinue && !animalsRunning)
                {
                    mEel.active = false;
                    mDog.active = true;
                    mMonkey.active = false;
                }
                if (monkeyImage != null && dogImage != null && eelImage != null)
                {
                    monkeyImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                    dogImage.rectTransform.sizeDelta = new Vector2(75.0f, 75.0f);
                    eelImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                }
                if (fraction >= 1.0f)
                    cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y + cameraYOffset, cam.transform.position.z);
            }
            else if (SelectedChar == (int)ActiveCharacter.Eel)
            {
                if (!DialogueManager.isOpen && DialogueManager.canContinue && !animalsRunning)
                {
                    mEel.active = true;
                    mDog.active = false;
                    mMonkey.active = false;
                }
                if (monkeyImage != null && dogImage != null && eelImage != null)
                {
                    monkeyImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                    dogImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                    eelImage.rectTransform.sizeDelta = new Vector2(75.0f, 75.0f);
                }
                if (fraction >= 1.0f)
                    cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y + cameraYOffset, cam.transform.position.z);
            }
        } else if (isHighlighting)
        {
            if(highlightedObject != null ) {
                mEel.active = false;
                mMonkey.active = false;
                mDog.active = false;

                timePassed = 0.0f;
                fraction = 0.0f;
                oldCameraPos = cam.transform.position;
                if (fraction >= 1.0f)
                {
                    cam.transform.position = new Vector3(highlightedObject.transform.position.x, highlightedObject.transform.position.y, cam.transform.position.z);
                }
                StartCoroutine(HighlightTheObject());
            }
            else
            { 
                SelectedChar = (int)ActiveCharacter.Monkey;
                highlightedObject = null;
            }
        }

        if (mMonkey.monkeyLevelComplete && mDog.levelCompleted && mEel.levelComplete)
        {
            StartCoroutine(FadeBeforenextLevel());
        }

        if (!animalsRunning && mMonkey.runRightCheck && mDog.runRightCheck && mEel.runRightCheck)
            StartCoroutine(RunToTheRight());
        if (fraction < 1.0f)
            MoveCamera();

        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x, minCameraPos.x, maxCameraPos.x), Mathf.Clamp(cam.transform.position.y, minCameraPos.y, maxCameraPos.y), cam.transform.position.z);
    }

    IEnumerator HighlightTheObject()
    {
        if (isElevator)
        {
            highlightedObject.GetComponent<Elevator>().highlighted = true;
        }
        if (isBridge)
        {
            highlightedObject.GetComponent<BridgeWheelmovement>().highlighted = true;
        }
        isHighlighting = true;
        yield return new WaitForSecondsRealtime(5f);
        isHighlighting = false;
        highlightedObject = null;
        isElevator = false;
        isBridge = false;
    }

    void MoveCamera()
    {
        timePassed += Time.deltaTime;
        fraction = timePassed / cameraTravelTime;
        if (!isHighlighting)
        {
            cam.transform.position = Vector3.Lerp(oldCameraPos, new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y + cameraYOffset, cam.transform.position.z), fraction);
        }
        else
        {
            fraction = timePassed / cameraTravelTime * objectTravelTimeMultiplier;
            cam.transform.position = Vector3.Lerp(oldCameraPos, new Vector3(highlightedObject.transform.position.x, highlightedObject.transform.position.y + cameraYOffset, cam.transform.position.z), fraction);
        }
    }

    IEnumerator FadeBeforenextLevel()
    {
        keepMusic = keepMusicWhenChangingScene;
        FadeToBlack.fadeIn = true;
        yield return new WaitForSeconds(1.5f);
        mMeny.NextLevel();
        StopAllCoroutines();
    }

    IEnumerator RunToTheRight()
    {
        animalsRunning = true;
        mEel.active = false;
        mMonkey.active = false;
        mDog.active = false;

        if (!mMonkey.carryingBucket)
        {
            mMonkey.ChangeState(mMonkey.pickingUpState);
            yield return new WaitForSeconds(1.5f);
        }

        mMonkey.runToRight = true;
        mDog.runToRight = true;
        yield return new WaitForSeconds(runToTheRightTime);
        mMonkey.runToRight = false;
        mDog.runToRight = true;
    }
}
