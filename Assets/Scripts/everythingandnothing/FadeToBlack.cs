using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public GameObject blackImageObject;
    Image blackImage;
    [Tooltip("The duration (in seconds) of the fade.")]
    public float fadeDuration = 1.0f;

    float timePassed;
    static public bool fadeIn;

    // Start is called before the first frame update
    void Awake()
    {
        blackImageObject.SetActive(true);
        blackImage = blackImageObject.GetComponent<Image>();
        timePassed = 1.0f;
        fadeIn = false;
        if (fadeDuration <= 0.0f)
        {
            fadeDuration = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
            FadeIn();
        else
            FadeOut();
    }

    void FadeIn()
    {
        timePassed += Time.deltaTime;
        if (timePassed > fadeDuration)
            timePassed = fadeDuration;
        if (!blackImageObject.activeSelf)
            blackImageObject.SetActive(true);
        blackImage.color = new Color(0.0f, 0.0f, 0.0f, timePassed / fadeDuration);
    }

    void FadeOut()
    {
        timePassed -= Time.deltaTime;
        if (timePassed < 0.0f)
        {
            if (blackImageObject.activeSelf)
                blackImageObject.SetActive(false);
            timePassed = 0.0f;
        }
        blackImage.color = new Color(0.0f, 0.0f, 0.0f, timePassed / fadeDuration);
    }
}
