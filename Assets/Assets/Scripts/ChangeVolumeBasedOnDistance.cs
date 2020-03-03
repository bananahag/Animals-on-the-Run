using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolumeBasedOnDistance : MonoBehaviour
{
    [Tooltip("The radius of the volime change thing.")]
    public float changeVolumeRadius = 10.0f;

    public enum TypeOfVolumeChange {QuietWhenCloseToObject, NoMusicPastTheLeftOfObject, NoMusicPastTheRightOfObject}

    float volume;

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
        distance = Vector3.Distance(transform.position, cameraPosition.position + cameraZOffset);
        if (changeVolumeRadius != 0.0f)
            volume = distance / changeVolumeRadius;
        if (volume > 1.0f)
            volume = 1.0f;
        else if (volume < 0.0f)
            volume = 0.0f;
    }
}
