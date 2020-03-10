using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    AudioSource audioSource;

    MenyUI menyUI;
    public Sprite openedSprite;
    public GameObject animal;
    public AudioClip openSFX;
    public float shakeSpeed = 50.0f;
    public float shakeDuration = 0.25f;

    [HideInInspector]
    public bool opened;

    bool shaking;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        menyUI = FindObjectOfType<MenyUI>().GetComponent<MenyUI>();
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
        if (!opened)
        {
            if(menyUI != null)
            {
                menyUI.AddScoreCount();
            }
            //audioSource.PlayOneShot(openSFX);
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            Instantiate(animal, transform.position, transform.rotation);
            StartCoroutine(Shake());
            opened = true;
        }
    }

    IEnumerator Shake()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
}
