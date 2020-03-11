using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Crossfade : MonoBehaviour
{

    public AudioMixer mixer;
    bool fading;
    public float Part1Duration;
    public float Part2Duration;
    public float Part3Duration;
    public void CrossfadeGroups(int list)
    {
        if (!fading)
        {
            StartCoroutine(Crossfades(list));
            
        }
    }

    IEnumerator Crossfades(int list)
    {
        fading = true;
        float currentTime = 0;
        if (list == 0)
        {
            while (currentTime <= Part1Duration)
            {
                currentTime += Time.deltaTime;

                mixer.SetFloat("MusicPart1", Mathf.Log10(Mathf.Lerp(1, 0.0001f, currentTime / Part1Duration)) * 20);
                mixer.SetFloat("MusicPart2", Mathf.Log10(Mathf.Lerp(0.0001f, 1, currentTime / Part1Duration)) * 20);

                yield return null;
            }
        }
        else if (list == 1)
        {
            while (currentTime <= Part2Duration)
            {
                currentTime += Time.deltaTime;

                mixer.SetFloat("MusicPart2", Mathf.Log10(Mathf.Lerp(1, 0.0001f, currentTime / Part2Duration)) * 20);
                mixer.SetFloat("MusicPart3", Mathf.Log10(Mathf.Lerp(0.0001f, 1, currentTime / Part2Duration)) * 20);

                yield return null;
            }
        }
        else
        {
            while (currentTime <= Part3Duration)
            {
                currentTime += Time.deltaTime;

                mixer.SetFloat("MusicPart3", Mathf.Log10(Mathf.Lerp(1, 0.0001f, currentTime / Part3Duration)) * 20);
                mixer.SetFloat("MusicPart1", Mathf.Log10(Mathf.Lerp(0.0001f, 1, currentTime / Part3Duration)) * 20);

                yield return null;
            }
        }
        

        fading = false;

    }
}
