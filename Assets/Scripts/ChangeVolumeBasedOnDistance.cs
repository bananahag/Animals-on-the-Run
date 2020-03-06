using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChangeVolumeBasedOnDistance : MonoBehaviour
{
    public AudioMixer audioMixer;
    [Tooltip("The radius of the volime change thing.")]
    public float changeVolumeRadius = 10.0f;

    public float maxVolume = 1.0f, minVolume = 0.0f;
    float distanceCheck;

    public enum TypeOfVolumeChange {QuietWhenCloseToObject, NoMusicPastTheLeftOfObject, NoMusicPastTheRightOfObject}
    public TypeOfVolumeChange typeOfVolumeChange = TypeOfVolumeChange.QuietWhenCloseToObject;

    float volume;
    bool inRange;

    Transform cameraPosition;
    Vector3 cameraZOffset;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraZOffset = new Vector3(0.0f, 0.0f, -cameraPosition.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (typeOfVolumeChange == TypeOfVolumeChange.NoMusicPastTheLeftOfObject || typeOfVolumeChange == TypeOfVolumeChange.NoMusicPastTheRightOfObject)
            distance = Mathf.Abs(transform.position.x - cameraPosition.position.x);
        else
            distance = Vector3.Distance(transform.position, cameraPosition.position + cameraZOffset);

        if (distance <= changeVolumeRadius)
            inRange = true;

        if (changeVolumeRadius != 0.0f)
        {
            if (typeOfVolumeChange == TypeOfVolumeChange.QuietWhenCloseToObject ||
                typeOfVolumeChange == TypeOfVolumeChange.NoMusicPastTheLeftOfObject && transform.position.x > cameraPosition.position.x ||
                typeOfVolumeChange == TypeOfVolumeChange.NoMusicPastTheRightOfObject && transform.position.x < cameraPosition.position.x)
            {
                distanceCheck = distance / changeVolumeRadius;
                //volume = (maxVolume * distanceCheck) + (minVolume * inverseDistanceCheck);
            }
            else
                volume = minVolume;
        }
        else
            volume = maxVolume;

        if (volume > maxVolume)
            volume = maxVolume;
        else if (volume < minVolume)
            volume = minVolume;

        if (audioMixer != null && inRange)
            audioMixer.SetFloat("Music_2D", Mathf.Log10(Mathf.Lerp(Mathf.Pow(10.0f, minVolume / 20.0f), Mathf.Pow(10.0f, maxVolume / 20.0f), distanceCheck)) * 20.0f);

        if (distance > changeVolumeRadius)
            inRange = false;
    }
}
