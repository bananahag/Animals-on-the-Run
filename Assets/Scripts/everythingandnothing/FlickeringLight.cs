using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    SpriteMask spriteMask;
    [Tooltip("The minimum and maximum time (in seconds) between the light flickering.")]
    public float timeBetweenFlickerMin = 1.0f, timeBetweenFlickerMax = 2.0f;

    float smallFlickerTimeMin = 0.1f, smallFlickerTimeMax = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        spriteMask.enabled = false;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        float waitTime = Random.Range(timeBetweenFlickerMin, timeBetweenFlickerMax);
        yield return new WaitForSeconds(waitTime);
        spriteMask.enabled = true;

        yield return new WaitForSeconds(0.1f);
        spriteMask.enabled = false;

        waitTime = Random.Range(smallFlickerTimeMin, smallFlickerTimeMax);
        yield return new WaitForSeconds(waitTime);
        spriteMask.enabled = true;

        yield return new WaitForSeconds(0.1f);
        spriteMask.enabled = false;
        StartCoroutine(Flicker());
    }
}
