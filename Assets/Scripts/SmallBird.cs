using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBird : MonoBehaviour
{
    public float flyingSpeed = 7.5f;
    public float destroyObjectAfterDuration = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyObjectAfterDuration);

        Vector2 flyingDirectoin = new Vector2(Random.Range(-1.5f, 1.5f), 1.0f);
        GetComponent<Rigidbody2D>().velocity = flyingDirectoin.normalized * flyingSpeed;
    }
}
