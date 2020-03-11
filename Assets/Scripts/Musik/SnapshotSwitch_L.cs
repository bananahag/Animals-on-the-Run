using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(BoxCollider))]
public class SnapshotSwitch_L : MonoBehaviour
{
    public AudioMixerSnapshot snapshotToPlay;
    public float snapshotFadetime;
    public Crossfade mCrossfade;
    public float duration = 1.0f;
    private int list = 0;
    /*  private void OnTriggerEnter()
      {
          if (snapshotToPlay != null)
              snapshotToPlay.TransitionTo(snapshotFadetime);
          BoxCollider col = GetComponent<BoxCollider>();
          col.enabled = false;
      }*/
    void Start()
    {
        
    }
    public void Update()
    {


        if (Input.GetKeyDown(KeyCode.J))
        {
            
            if (list > 2)
            {
                list = 0;
            }
            mCrossfade.CrossfadeGroups(list);
            list++;
        }


    }
}