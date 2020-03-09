using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyDropsEel : MonoBehaviour
{
    BoxCollider2D boxCol2D;

    // Start is called before the first frame update
    void Start()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
    }

    public void Deactivate()
    {
        boxCol2D.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monkey")
        {
            if (other.gameObject.GetComponent<MonkeyBehavior>() != null)
                other.gameObject.GetComponent<MonkeyBehavior>().DropEel();
        }
    }
}
