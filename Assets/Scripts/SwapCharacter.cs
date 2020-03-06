using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacter : MonoBehaviour
{
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
        fraction = 0.0f;
    }
    void Start()
    {
        SelectedChar = 0;
        
        cam = Camera.main;
        mMeny = GameObject.Find("Meny").GetComponent<MenyUI>();

        
        oldCameraPos = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        print(fraction);
        if (Input.GetKeyDown(KeyCode.LeftShift))
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

        if (SelectedChar == (int)activeCharacter.Monkey)
        {
            mEel.active = false;
            mDog.active = false;
            mMonkey.active = true;
            if (fraction >= 1.0f)
                cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z);
        }

        else if (SelectedChar == (int)activeCharacter.Dog)
        {
            mEel.active = false;
            mMonkey.active = false;
            mDog.active = true;
            if (fraction >= 1.0f)
                cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z);
        }
        else if (SelectedChar == (int)activeCharacter.Eel)
        {
            mDog.active = false;
            mMonkey.active = false;
            mEel.active = true;
            if (fraction >= 1.0f)
                cam.transform.position = new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z);
        }


        if (mMonkey.monkeyLevelComplete && mDog.levelCompleted)
        {
            mMeny.NextLevel();
        }
        if (fraction < 1.0f)
            MoveCamera();
    }

    void MoveCamera()
    {
        timePassed += Time.deltaTime;
        fraction = timePassed / cameraTravelTime;
        cam.transform.position = Vector3.Lerp(oldCameraPos, new Vector3(characters[SelectedChar].transform.position.x, characters[SelectedChar].transform.position.y, cam.transform.position.z), fraction);
    }
}
