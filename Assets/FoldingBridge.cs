using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldingBridge : MonoBehaviour
{
    float maxRadiansDelta = 15;
    float maxMagnitudeDelta = 1;
    Vector2 rotationpoint;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rotationpoint = new Vector2(sr.sprite.bounds.size.x * 0, sr.sprite.bounds.size.y * 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
