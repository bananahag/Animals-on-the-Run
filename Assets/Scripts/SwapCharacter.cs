using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacter : MonoBehaviour
{
    public List<GameObject> characters;
    private Monkey mMonkey;
    private Dog mDog;
    public int SelectedChar;
    public enum activeCharacter
    {
        Monkey,
        Dog,
    }
    // Start is called before the first frame update
    void Start()
    {
        SelectedChar = 0;
        mMonkey = characters[0].GetComponent<Monkey>();
        mDog = characters[1].GetComponent<Dog>();
        
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
        }
        if (SelectedChar == (int)activeCharacter.Dog)
        {
            characters[0].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            mMonkey.enabled = false;
            mDog.enabled = true;
            
        }
        else if (SelectedChar == (int)activeCharacter.Monkey)
        {
            mDog.enabled = false;
            mMonkey.enabled = true;
        }
    }
}
