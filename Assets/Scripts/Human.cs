using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float speed = 3;
    public bool moving;
    public bool charmed;

    // Start is called before the first frame update
    void Start()
    {
        charmed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (charmed)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }

        if (moving)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
    }
}
