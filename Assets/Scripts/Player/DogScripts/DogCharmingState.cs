using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogCharmingState : DogState
{
    [Tooltip("Audio source that plays and loops while the dog is charming.")]
    public AudioSource charmingSource;

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        charmingSource.loop = true;
        charmingSource.Play();
        dog.charmingHuman = true;
        if (dog.wet)
        {
            dog.animator.Play("Charming Wet");
            dog.human.GetComponent<Human>().charmed = false;
        }
        else
        {
            
            dog.animator.Play("Charming");
            dog.human.GetComponent<Human>().charmed = true;
            
        }
        dog.movement = new Vector2(0, 0);
    }

    public override void Exit()
    {
        charmingSource.Stop();
        dog.human.GetComponent<Human>().charmed = false;
    }

    public override void Update()
    {
        if(Input.GetButtonDown("Interact") && dog.charmingHuman)
        {
            
            dog.charmingHuman = false;
        }
        if(dog.transform.position.x <= dog.human.transform.position.x) {
            dog.facingRight = true;
        }
        else if(dog.transform.position.x > dog.human.transform.position.x)
        {
            dog.facingRight = false;
        }
    }

    public override void FixedUpdate()
    {
        if (!dog.charmingHuman)
        {
            dog.ChangeState(dog.groundedState);
        }
    }

    /*public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            Debug.Log("trigger in charming leaving human");
            dog.closeToHuman = false;
            dog.charmingHuman = false;
        }
    }*/

}
