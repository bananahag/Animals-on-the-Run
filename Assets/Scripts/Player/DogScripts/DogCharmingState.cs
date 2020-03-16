using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DogCharmingState : DogState
{

    [Tooltip("Audio source that plays and loops while the dog is charming.")]
    public AudioSource charmingSource;
    public float charmDistance = 100.0f;
    List<GameObject> charmedHumans;
    

    public override void OnValidate(DogBehaviour dog)
    {
        this.dog = dog;
    }

    public override void Enter()
    {
        charmedHumans = new List<GameObject>();
        charmingSource.loop = true;
        charmingSource.Play();
        //dog.charmingHuman = true;
           
        dog.animator.Play("Charming");
        dog.movement = new Vector2(0, 0);
    }

    public override void Exit()
    {
        charmingSource.Stop();
        charmedHumans.Clear();
    }

    public override void Update()
    {

        RaycastHit2D[] humanSearch = Physics2D.RaycastAll(dog.transform.position - dog.charmDistanceVector, Vector2.right, charmDistance, dog.humanLayerMask);
        foreach (RaycastHit2D hit in humanSearch)
        {
            Debug.Log(charmedHumans.Count);
            if (hit.collider.gameObject.tag != "Button")
            {
                if (!hit.collider.gameObject.GetComponentInParent<Human>().charmed)
                {
                charmedHumans.Add(hit.collider.gameObject);
                hit.collider.gameObject.GetComponentInParent<Human>().charmed = true;
                    hit.collider.gameObject.GetComponentInParent<Human>().SwitchHumanState(Human.HumanState.Charmed);
                }
            }
        }

        if (Input.GetButtonDown("Light"))
        {
            for (int i = 0; i < charmedHumans.Count; i++)
            {
                charmedHumans[i].gameObject.GetComponentInParent<Human>().charmed = false;
                charmedHumans[i].gameObject.GetComponentInParent<Human>().SwitchHumanState(Human.HumanState.Moving);
            }
            dog.ChangeState(dog.groundedState);

        }


            /*if(Input.GetButtonDown("Interact") && dog.charmingHuman)
            {

                dog.charmingHuman = false;
            }
            if(dog.transform.position.x <= dog.human.transform.position.x) {
                dog.facingRight = true;
            }
            else if(dog.transform.position.x > dog.human.transform.position.x)
            {
                dog.facingRight = false;
            }*/
        }

    public override void FixedUpdate()
    {
       /* if (!dog.charmingHuman)
        {
            dog.ChangeState(dog.groundedState);
        }*/
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
