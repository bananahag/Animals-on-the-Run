using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    public AudioSource cutsceneSource;

    VideoPlayer videoPlayer;
    public string sceneAfterVideoName;
    public float waitTimeBeforeLoadingNewScene = 0.0f;

    public bool intro;

    bool checkVideoPlayingState, hasStartedAudio;

    // Start is called before the first frame update
    void Start()
    {
        checkVideoPlayingState = false;
        hasStartedAudio = false;
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(WaitBeforeCheckingVideoPlayingState());
        videoPlayer.Play();
        if (intro)
            MenyUI.scene++;
        else
            MenyUI.scene = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkVideoPlayingState && !videoPlayer.isPlaying)
        {
            StartCoroutine(LoadNewScene());
            checkVideoPlayingState = false;
        }

        if (videoPlayer.isPlaying && !hasStartedAudio)
        {
            if (cutsceneSource != null)
                cutsceneSource.Play();
            hasStartedAudio = true;
        }
    }

    IEnumerator WaitBeforeCheckingVideoPlayingState()
    {
        yield return new WaitForSeconds(1.0f);
        checkVideoPlayingState = true;
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(waitTimeBeforeLoadingNewScene);
        SceneManager.LoadScene(sceneAfterVideoName);
    }
}
