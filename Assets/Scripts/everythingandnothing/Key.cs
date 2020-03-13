using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Tooltip("The door the key opens.")]
    public GameObject lockedDoor;

    [Tooltip("Game object with the sound effect that spawns when they key is collected.")]
    public GameObject keySoundEffect;

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
            if (keySoundEffect != null)
                Instantiate(keySoundEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
