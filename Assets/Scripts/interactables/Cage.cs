using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [Tooltip("Audio source that plays when the cage opens.")]
    public AudioSource openSource;
    [Tooltip("Audio source that plays the audio clips.")]
    public AudioSource tweetSource;
    [Tooltip("Audio clips that plays when the bird inside the cage makes a tweet sound.")]
    public AudioClip[] tweetClips;
    [Tooltip("The minimum and the maximum time (in seconds) between the tweet sounds.")]
    public float minTime = 1.0f, maxTime = 5.0f;
    [Tooltip("The minimum and the maximum volume of the tweet sounds.")]
    public float minVolume = 1.0f, maxVolume = 1.0f;
    [Tooltip("The minimum and the maximum pitch of the tweet sounds.")]
    public float minPitch = 1.0f, maxPitch = 1.0f;

    ScoreCounter scoreCounter;

    public GameObject Dialog;
    public Sprite openedSprite;
    public GameObject animal;
    public float shakeSpeed = 50.0f;
    public float shakeDuration = 0.25f;

    [HideInInspector]
    public bool opened;

    bool shaking;

    // Start is called before the first frame update
    void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>().GetComponent<ScoreCounter>();
        StartCoroutine(TweetSound());
        Dialog.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shaking)
        {
            float AngleAmount = (Mathf.Cos(Time.time * shakeSpeed) * 180) / Mathf.PI * 0.5f;
            AngleAmount = Mathf.Clamp(AngleAmount, -15, 15);
            transform.localRotation = Quaternion.Euler(0, 0, AngleAmount);
        }
    }

    public void Open()
    {
        if (!opened)
        {
            if(scoreCounter != null)
            {
                scoreCounter.AddScoreCount();
            }
            openSource.Play();
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            Instantiate(animal, transform.position, transform.rotation);
            StartCoroutine(Shake());
            opened = true;
            Dialog.SetActive(true);
        }
    }

    IEnumerator Shake()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    IEnumerator TweetSound()
    {
        if (tweetClips.Length > 0)
        {
            int randomClip = Random.Range(0, tweetClips.Length);
            tweetSource.volume = Random.Range(minVolume, maxVolume);
            tweetSource.pitch = Random.Range(minPitch, maxPitch);
            tweetSource.clip = tweetClips[randomClip];
            tweetSource.Play();
        }
        float waitTime = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitTime);
        if (!opened)
            StartCoroutine(TweetSound());
    }
}
