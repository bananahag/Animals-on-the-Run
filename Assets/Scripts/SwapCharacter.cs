using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacter : MonoBehaviour
{
    MenyUI mMeny;
    public List<GameObject> characters;
    private Monkey mMonkey;
    private Dog mDog;
    public int SelectedChar;
    private Camera cam;
    
    public enum activeCharacter
    {
        Monkey,
        Dog,
    }

    private void Awake()
    {

        mMonkey = characters[0].GetComponent<Monkey>();
        mDog = characters[1].GetComponent<Dog>();
    }
    void Start()
    {
        SelectedChar = 0;
        
        cam = Camera.main;
        mMeny = GameObject.Find("Meny").GetComponent<MenyUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
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

        if (SelectedChar == (int)activeCharacter.Dog)
        {
           
            mMonkey.notActive = true;
            mDog.notActive = false;
            cam.transform.position = new Vector3(characters[1].transform.position.x, characters[1].transform.position.y, -10);
        }

        else if (SelectedChar == (int)activeCharacter.Monkey)
        {
            
            mDog.notActive = true;
            mMonkey.notActive = false;
            cam.transform.position = new Vector3(characters[0].transform.position.x, characters[0].transform.position.y, -10);
        }

        if (mMonkey.monkeyLevelComplete && mDog.dogLevelComplete)
        {
            mMeny.NextLevel();
        }
        
    }
}
