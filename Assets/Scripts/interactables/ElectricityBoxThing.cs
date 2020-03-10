using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityBoxThing : MonoBehaviour
{
    public AudioSource activateSource;
    public GameObject lever;

    bool activated;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("Elskap Inaktiv");
        if (lever != null)
            lever.GetComponent<ButtonOrLever>().Charge(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Electricity" && !activated)
        {
            if (activateSource != null)
                activateSource.Play();
            GetComponent<Animator>().Play("Elskap Aktiv");
            lever.GetComponent<ButtonOrLever>().Charge(true);
            activated = true;
        }
    }
}
