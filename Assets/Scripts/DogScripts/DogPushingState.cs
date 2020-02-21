using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogPushingState : DogState
{
    bool canMoveObject;

    
    public float leftInteractPos = -1f;
    public float rightInteractPos = 1f;
    public float yInteractOffsetAbove = 0.9f; //Ge förklaring för båda.
    public float yInteractOffsetBelow = -0.9f;
    private float interactPosition;



    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        Debug.Log("Dog In Pushing State");
        dog.affectedObject.GetComponent<MovableObject>().Pickup(dog.gameObject);
        dog.movingObject = true;
    }
    public override void Update()
    {

    }

    public override void Exit()
    {

    }


    public override void FixedUpdate()
    {

    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovableObject"))
        {
            dog.affectedObject = other.gameObject;

            Vector3 dir = dog.affectedObject.transform.position - dog.transform.position;
            Debug.Log(dir);
            if (dir.y >= yInteractOffsetAbove || dir.y <= yInteractOffsetBelow)
            {
                canMoveObject = false;
            }
            else
            {
                canMoveObject = true;
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {

    }

}
