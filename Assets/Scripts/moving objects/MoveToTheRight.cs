using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTheRight : MonoBehaviour
{

    public float travelSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(1 * travelSpeed, 0.0f);
    }
}
