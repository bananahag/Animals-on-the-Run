using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public string sceneAfterVideoName;
    public float waitTimeBeforeLoadingNewScene = 0.0f;

    bool checkVideoPlayingState;

    // Start is called before the first frame update
    void Start()
    {
        checkVideoPlayingState = false;
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(WaitBeforeCheckingVideoPlayingState());
    }

    // Update is called once per frame
    void Update()
    {
        if (checkVideoPlayingState && !videoPlayer.isPlaying)
        {
            StartCoroutine(LoadNewScene());
            checkVideoPlayingState = false;
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
