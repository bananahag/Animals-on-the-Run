using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{
    public GameObject lanternLight;
    public int randomFlashTime;
    public float flashTime = 1f;
    private bool lightIsActive = true;
    bool check;
    int waitTime;


    void Start()
    {
        //StartCoroutine(WaitForFlash());
        waitTime = Random.Range(4, randomFlashTime);
    }


    IEnumerator WaitForFlash(int waitTime)
    {
        check = true;
        

        yield return new WaitForSeconds(waitTime);
       
        check = false;
        LanternFlash();
    }

    void LanternFlash()
    {
        if (lanternLight.gameObject.activeInHierarchy)
        {
            lanternLight.gameObject.SetActive(false);
        }
        else
        {
            lanternLight.gameObject.SetActive(true);
        }
    }

   IEnumerator FlashTime()
    {
        yield return new WaitForSeconds(flashTime);
        lanternLight.gameObject.SetActive(true);
        lightIsActive = true;
    }

    
    void Update()
    {
        /*if (lightIsActive == true)
         {
             StartCoroutine(WaitForFlash());
         }
        else
         {
             lanternLight.gameObject.SetActive(true);
             lightIsActive = true;
         }*/
        if (!check)
        {
            waitTime = Random.Range(4, randomFlashTime);
            StartCoroutine(WaitForFlash(waitTime));
        }
    }
}
