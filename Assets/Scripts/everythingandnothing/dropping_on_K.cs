using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropping_on_K : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
