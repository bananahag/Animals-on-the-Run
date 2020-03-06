using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Tooltip("The audio source that plays when the door opens.")]
    public AudioSource openSource;
    [Tooltip("The sprite that is used by the door when it gets unlocked.")]
    public Sprite openedSprite;

    public enum KeyColor {Orange, Purple}
    [Tooltip("Determines what kind of key gets removed from the UI.")]
    public KeyColor keyColor; 

    BoxCollider2D boxCol2D;
    bool canBeOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monkey" && canBeOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            boxCol2D.enabled = false;
            if (keyColor == KeyColor.Orange)
                FindObjectOfType<KeyUI>().ViewOrangeKey(false);
            else
                FindObjectOfType<KeyUI>().ViewPurpleKey(false);
            openSource.Play();
        }
    }

    public void KeyCollected()
    {
        canBeOpened = true;
    }
}
