using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    AudioSource musicSource;
    bool stopMusic;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!SwapCharacter.keepMusic)
            stopMusic = true;
        if (stopMusic)
            musicSource.volume -= 0.02f;
        if (musicSource.volume <= 0.0f)
            Destroy(gameObject);
    }

    public void DestroyMusicObject()
    {
        Destroy(gameObject);
    }
}
