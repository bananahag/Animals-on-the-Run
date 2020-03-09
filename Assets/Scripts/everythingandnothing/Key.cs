using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Tooltip("The door the key opens.")]
    public GameObject lockedDoor;

    public enum KeyColor { Orange, Purple }
    [Tooltip("Determines the color of they key and what kind of key gets added to the UI.")]
    public KeyColor keyColor;

    private void Start()
    {
        if (keyColor == KeyColor.Orange)
            GetComponent<Animator>().Play("Orange Key");
        else
            GetComponent<Animator>().Play("Purple Key");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Monkey")
        {
            lockedDoor.GetComponent<LockedDoor>().KeyCollected();
            if (keyColor == KeyColor.Orange)
                FindObjectOfType<KeyUI>().ViewOrangeKey(true);
            else
                FindObjectOfType<KeyUI>().ViewPurpleKey(true);
            Destroy(gameObject);
        }
    }
}
