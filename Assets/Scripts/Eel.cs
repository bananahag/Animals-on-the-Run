using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel : MonoBehaviour
{
    public GameObject LightObj;
    public GameObject Electricity;
    public bool canEelActivate = false;

    bool lightIsActive;
    

    void Start()
    {
        LightObj.SetActive(false);
        lightIsActive = false;
    }

    void Update()
    {
        EelLight();
        EelElectricity();

    }
    void EelLight()
    {
        if (Input.GetButtonDown("Light"))
        {
            if (lightIsActive)
            {
                lightIsActive = false;
                LightObj.SetActive(false);
            }
            else
            {
                lightIsActive = true;
                LightObj.SetActive(true);
            }
            
        }
    }

    void EelElectricity()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("yep");
        }
    }
}
