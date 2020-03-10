using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCharacter : MonoBehaviour
{
    public Image monkeyImage, dogImage, eelImage;

    MenyUI mMeny;
    public List<GameObject> characters;
    private MonkeyBehavior mMonkey;
    private DogBehaviour mDog;
    private Eel mEel;
    public int SelectedChar;
    private Camera cam;
    Vector3 oldCameraPos;

    public float cameraTravelTime = 1.0f;
    float timePassed;
    float fraction;
    int targetPosition;

    public Vector2 minCameraPos, maxCameraPos;

    public enum activeCharacter
    {
        Monkey,
        Dog,
        Eel,
    }

    private void Awake()
    {
        mMonkey = characters[0].GetComponent<MonkeyBehavior>();
        mDog = characters[1].GetComponent<DogBehaviour>();
        mEel = characters[2].GetComponent<Eel>();
        timePassed = 0.0f;
        fraction = 1.0f;
    }
    void Start()
    {
        SelectedChar = 0;
        
        cam = Camera.main;

        
        oldCameraPos = cam.transform.position;
        mMeny = GameObject.Find("Meny").GetComponent<MenyUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Swap1"))
        {
            timePassed = 0.0f;
            fraction = 0.0f;
            oldCameraPos = cam.transform.position;
            if (SelectedChar < characters.Count -1)
            {
                SelectedChar++;
            }
            else
            {
                SelectedChar = 0;
            }

            if (SelectedChar == (int)activeCharacter.Dog)
            {
            characters[0].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[0].GetComponent<Rigidbody2D>().velocity.y, 0);
            }
            else if (SelectedChar == (int)activeCharacter.Monkey)
            {
                characters[1].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[1].GetComponent<Rigidbody2D>().velocity.y, 0);
            }
        }
        else if (Input.GetButtonDown("Swap2"))
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

            if (SelectedChar == (int)activeCharacter.Dog)
            {
                characters[0].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[0].GetComponent<Rigidbody2D>().velocity.y, 0);
            }
            else if (SelectedChar == (int)activeCharacter.Monkey)
            {
                characters[1].GetComponent<Rigidbody2D>().velocity = new Vector3(0, characters[1].GetComponent<Rigidbody2D>().velocity.y, 0);
            }
        }

        if (SelectedChar == (int)activeCharacter.Monkey)
        {
            mEel.active = false;
            mDog.active = false;
            mMonkey.active = true;
            if (monkeyImage != null && dogImage != null && eelImage != null)
            {
                monkeyImage.rectTransform.sizeDelta = new Vector2(75.0f, 75.0f);
                dogImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                eelImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
            }
            if (fraction >= 1.0f)
                cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z);
        }

        else if (SelectedChar == (int)activeCharacter.Dog)
        {
            mEel.active = false;
            mMonkey.active = false;
            mDog.active = true;
            if (monkeyImage != null && dogImage != null && eelImage != null)
            {
                monkeyImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                dogImage.rectTransform.sizeDelta = new Vector2(75.0f, 75.0f);
                eelImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
            }
            if (fraction >= 1.0f)
                cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z);
        }
        else if (SelectedChar == (int)activeCharacter.Eel)
        {
            mDog.active = false;
            mMonkey.active = false;
            mEel.active = true;
            if (monkeyImage != null && dogImage != null && eelImage != null)
            {
                monkeyImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                dogImage.rectTransform.sizeDelta = new Vector2(25.5f, 25.5f);
                eelImage.rectTransform.sizeDelta = new Vector2(75.0f, 75.0f);
            }
            if (fraction >= 1.0f)
                cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z);
        }


        if (mMonkey.monkeyLevelComplete && mDog.levelCompleted)
        {
            mMeny.NextLevel();
        }
        if (fraction < 1.0f)
            MoveCamera();

        cam.transform.position = new Vector3(Mathf.Clamp(cam.transform.position.x, minCameraPos.x, maxCameraPos.x), Mathf.Clamp(cam.transform.position.y, minCameraPos.y, maxCameraPos.y), cam.transform.position.z);
    }

    void MoveCamera()
    {
        timePassed += Time.deltaTime;
        fraction = timePassed / cameraTravelTime;
        cam.transform.position = Vector3.Lerp(oldCameraPos, new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z), fraction);
    }
}
