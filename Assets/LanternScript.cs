using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{
    public GameObject lanternLight;
    public int randomFlashTime;
    public float flashTime = 0.5f; 
   
    void Start()
    {
        StartCoroutine(Waiter());
    }


    IEnumerator Waiter()
    {
        int waitTime = Random.Range(0, randomFlashTime);

        yield return new WaitForSeconds(waitTime);
        LanternFlash();
    }

    void LanternFlash()
    {
        lanternLight.gameObject.SetActive(false);
    }

    IEnumerator FlashTimer()
    {
        yield return new WaitForSeconds(flashTime);
    }
    void Update()
    {
       
    }
}
