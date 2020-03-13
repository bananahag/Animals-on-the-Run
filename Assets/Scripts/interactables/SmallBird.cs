using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBird : MonoBehaviour
{
    [Tooltip("Audio source that plays when the bird is set free.")]
    public AudioSource tweetSource;
    [Tooltip("Audio source that plays the flap audio clips.")]
    public AudioSource flapSource;
    [Tooltip("Audio clips that plays when the bird flaps its wings.")]
    public AudioClip[] flapClips;
    [Tooltip("The minimum and the maximum volume of the tweet sounds.")]
    public float minVolume = 0.9f, maxVolume = 1.1f;
    [Tooltip("The minimum and the maximum pitch of the tweet sounds.")]
    public float minPitch = 0.9f, maxPitch = 1.1f;

    public float flyingSpeed = 7.5f;
    public float destroyObjectAfterDuration = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyObjectAfterDuration);
        tweetSource.Play();

        Vector2 flyingDirectoin = new Vector2(Random.Range(-1.5f, 1.5f), 1.0f);
        GetComponent<Rigidbody2D>().velocity = flyingDirectoin.normalized * flyingSpeed;
    }

    public void PlayFlapSound()
    {
        if (flapClips.Length > 0)
        {
            int randomClip = Random.Range(0, flapClips.Length);
            flapSource.volume = Random.Range(minVolume, maxVolume);
            flapSource.pitch = Random.Range(minPitch, maxPitch);
            flapSource.clip = flapClips[randomClip];
            flapSource.Play();
        }
    }
}
