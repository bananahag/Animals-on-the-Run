using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMonkeyDropsEel : MonoBehaviour
{
    [Tooltip("The object with the ''Monkey Drop Eel'' script that gets disabled.")]
    public GameObject NoBucketZone;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MovableObject" || other.gameObject.tag == "MoveableObject")
        {
            if (NoBucketZone.GetComponent<MonkeyDropsEel>() != null)
                NoBucketZone.GetComponent<MonkeyDropsEel>().Deactivate();
        }
    }
}
