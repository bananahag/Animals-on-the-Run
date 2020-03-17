using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour
{
    public GameObject lanternLight;
    [Tooltip("Slumpmässig tid det tar innan lampan slocknar")]
    public int randomFlashTime;
    [Tooltip("Tiden som lampan är släckt")]
    public float flashTime = 1f;
    [Tooltip("Minsta möjliga tiden mellan på/av")]
    public int minTime = 1;
    int waitTime;


    void Start()
    {
        StartCoroutine(WaitForFlash());
    }


    IEnumerator WaitForFlash()
    {
        waitTime = Random.Range(minTime, randomFlashTime);

        yield return new WaitForSeconds(waitTime);
       
        LanternOff();
    }

    void LanternOff()
    {
        lanternLight.gameObject.SetActive(false);
        StartCoroutine(FlashTime());
    }

   IEnumerator FlashTime()
    {
        yield return new WaitForSeconds(flashTime);

        LanternOn();
    }

    void LanternOn()
    {
        lanternLight.gameObject.SetActive(true);
        StartCoroutine(WaitForFlash());
    }

    
    void Update()
    {

    }
}
