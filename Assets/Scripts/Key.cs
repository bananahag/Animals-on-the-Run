using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Tooltip("The door the key opens.")]
    public GameObject lockedDoor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monkey")
        {
            lockedDoor.GetComponent<LockedDoor>().KeyCollected();
            Destroy(gameObject);
        }
    }
}
