﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel : MonoBehaviour
{
    [Tooltip("The sound effect that plays when the eel uses its electicity.")]
    public AudioClip electricitySFX;
    [Tooltip("The sound effect that plays when the eel uses its light")]
    public AudioClip lightSFX;
    [Tooltip("The sound effect that plays when the eel lands on the ground after falling.")]
    public AudioClip landingSFX;

    [Tooltip("If either of these transforms touches an object with the ''Ground'' layer the eel will be grounded.")]
    public Transform groundCheckLeft = null, groundCheckRight = null;

    public float lightScaleSpeedX = 0.5f, lightScaleSpeedY = 0.5f;
    [Tooltip("The time (in seconds) the eel is stopped when using electricity.")]
    public float electricityTime = 1.0f;
    [Tooltip("The time (in seconds) the eel is stopped when landing.")]
    public float landingTime = 0.5f;

    public GameObject LightObj;
    public GameObject Electricity;

    [HideInInspector]
    public bool grounded;
    bool lightIsActive;
    bool canAct;
    float flashTime = 0.05f;

    float targetLightScaleX, targetLightScaleY;
    float currentScaleX, currentScaleY;

    AudioSource audioSource;
    Animator animator;

    void Start()
    {
        LightObj.SetActive(true);
        lightIsActive = false;
        canAct = true;
        targetLightScaleX = LightObj.transform.localScale.x;
        targetLightScaleY = LightObj.transform.localScale.y;

        LightObj.transform.localScale = new Vector2(0.0f, 0.0f);

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        EelLight();
        EelElectricity();
        if (canAct)
            EelAnimations();
        GroundCheck();

    }

    private void FixedUpdate()
    {
        ChangeLightSize();
    }

    void GroundCheck()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (!grounded)
            {
                canAct = false;
                audioSource.PlayOneShot(landingSFX);
                animator.Play("Placeholder Eel Land");
                StartCoroutine(LandingTimer());
            }
            grounded = true;
        }
        else
        {
            canAct = false;
            grounded = false;
        }
    }

    void EelLight()
    {
        if (Input.GetButtonDown("Light") && canAct && grounded)
        {
            audioSource.PlayOneShot(lightSFX);
            if (lightIsActive)
            {
                lightIsActive = false;
                animator.Play("Placeholder Eel Idle");
            }
            else
            {
                lightIsActive = true;
                animator.Play("Placeholder Eel Light");
            }
        }
    }

    void EelAnimations()
    {
        if (grounded)
        {
            if (lightIsActive)
                animator.Play("Placeholder Eel Light");
            else
                animator.Play("Placeholder Eel Idle");
        }
        else
        {
            animator.Play("Placeholder Eel Fall");
        }
    }

    void EelElectricity()
    {
        if (Input.GetButtonDown("Interact") && canAct && grounded)
        {
            audioSource.PlayOneShot(electricitySFX);
            canAct = false;
            StartCoroutine(ElectricityTimer());
            animator.Play("Placeholder Eel Idle");
            animator.Play("Placeholder Eel Electricity");
        }
    }

    void SpawnElectricityObject()
    {
        Instantiate(Electricity, transform.position, transform.rotation);
        StartCoroutine(Flash());
    }

    void ChangeLightSize()
    {
        if (canAct)
        {
            if (lightIsActive)
            {
                currentScaleX += lightScaleSpeedX;
                currentScaleY += lightScaleSpeedY;
                if (currentScaleX > targetLightScaleX)
                    currentScaleX = targetLightScaleX;
                if (currentScaleY > targetLightScaleY)
                    currentScaleY = targetLightScaleY;
            }
            else
            {
                currentScaleX -= lightScaleSpeedX;
                currentScaleY -= lightScaleSpeedY;
                if (currentScaleX < 0.0f)
                    currentScaleX = 0.0f;
                if (currentScaleY < 0.0f)
                    currentScaleY = 0.0f;
            }
        }
        
        LightObj.transform.localScale = new Vector2(currentScaleX, currentScaleY);
    }

    IEnumerator ElectricityTimer()
    {
        yield return new WaitForSeconds(electricityTime);
        canAct = true;
    }

    IEnumerator Flash()
    {
        float flashScaleX = 0.0f, flashScaleY = 0.0f;
        if (!lightIsActive)
        {
            flashScaleX = targetLightScaleX / 2;
            flashScaleY = targetLightScaleY / 2;
        }
        else
        {
            flashScaleX = currentScaleX * 1.1f;
            flashScaleY = currentScaleY * 1.1f;
        }

        for (int i = 0; i < 2; i++)
        {
            currentScaleX = flashScaleX;
            currentScaleY = flashScaleY;
            yield return new WaitForSeconds(flashTime);
            if (!lightIsActive)
            {
                currentScaleX = 0.0f;
                currentScaleY = 0.0f;
            }
            else
            {
                currentScaleX = targetLightScaleX;
                currentScaleY = targetLightScaleY;
            }
            yield return new WaitForSeconds(flashTime);
        }
    }

    IEnumerator LandingTimer()
    {
        yield return new WaitForSeconds(landingTime);
        canAct = true;
    }
}