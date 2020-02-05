﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip openSFX;
    public float shakeSpeed = 50.0f;
    public float shakeDuration = 0.5f;

    bool shaking;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        //audioSource.PlayOneShot(openSFX);
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
}
