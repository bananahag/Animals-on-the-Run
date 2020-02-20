using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel : MonoBehaviour
{
    public GameObject LightObj;
    public GameObject ElectricThing;
    public bool canEelActivate = false;
    

    void Start()
    {
        LightObj.SetActive(false);
    }

    void Update()
    {
        EelLight();
        EelElectricity();

    }
    void EelLight()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LightObj.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LightObj.SetActive(false);
        }
    }

    void EelElectricity()
    {
        if (Input.GetKeyDown(KeyCode.F) && canEelActivate == true)
        {
            Debug.Log("yep");
        }
    }

    void OnTriggerEnter2D(Collider other)
    {
        if (other.gameObject.tag == "ElectricThing")
        {
            canEelActivate = true;
        }
    }
}
