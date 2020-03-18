using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eel : MonoBehaviour
{
    [Tooltip("The audio source that plays when the eel uses its electicity.")]
    public AudioSource electricitySource;
    [Tooltip("The audio source that plays when the eel uses its light")]
    public AudioSource lightSource;
    [Tooltip("The audio source that plays when the eel lands on the ground after falling.")]
    public AudioSource landingSource;

    public Collider2D collider;

    [Tooltip("If either of these transforms touches an object with the ''Ground'' layer the eel will be grounded.")]
    public Transform groundCheckLeft = null, groundCheckRight = null;

    public float lightScaleSpeedX = 0.5f, lightScaleSpeedY = 0.5f;
    [Tooltip("The time (in seconds) the eel is stopped when using electricity.")]
    public float electricityTime = 1.0f;
    [Tooltip("The time (in seconds) the eel is stopped when landing.")]
    public float landingTime = 0.5f;
    [Tooltip("The offset position of the eel when it gets picked up by the monkey.")]
    public Vector3 monkeyCarryOffset = new Vector3(0.0f, 0.5f, 0.0f);

    public GameObject LightObj;
    public GameObject Electricity;
    [HideInInspector]
    public GameObject monkey;

    [HideInInspector]
    public bool grounded;
    bool lightIsActive;
    bool canAct;
    float flashTime = 0.05f;
    bool pickedUp;
    [HideInInspector]
    public bool active = false;

    [HideInInspector]
    public bool levelComplete = false, runRightCheck;

    bool canPlayLightSource;
    float targetLightScaleX, targetLightScaleY;
    float currentScaleX, currentScaleY;
    float startGravity;
    bool loopLightOnce;
    Animator animator;

    void Start()
    {
        startGravity = GetComponent<Rigidbody2D>().gravityScale;
        LightObj.SetActive(true);
        lightIsActive = false;
        canAct = true;
        targetLightScaleX = LightObj.transform.localScale.x;
        targetLightScaleY = LightObj.transform.localScale.y;

        LightObj.transform.localScale = new Vector2(0.0f, 0.0f);

        canPlayLightSource = true;
        lightSource.loop = true;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        
        EelLight();
        EelElectricity();
        if (canAct)
            EelAnimations();
        
        if (pickedUp)
            PickedUp();
        else
            GroundCheck();


        
    }

    private void FixedUpdate()
    {
        ChangeLightSize();
    }

    void GroundCheck()
    {
        if (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (!grounded)
            {
                canAct = false;
                landingSource.Play();
                animator.Play("Placeholder Eel Land");
                StartCoroutine(LandingTimer());
            }
            
            grounded = true;
        }
        else
        {
            animator.Play("Placeholder Eel Fall");
            canAct = false;
            grounded = false;
        }
    }

    void EelLight()
    {
        if (Input.GetButtonDown("Light") && canAct && grounded && active)
        {
            if (lightIsActive)
            {
                animator.SetBool("LoopLightOnce", false);
                lightIsActive = false;
                animator.Play("Placeholder Eel Light");
            }
            else 
            {
                animator.SetBool("LoopLightOnce", true);
                lightIsActive = true;
                animator.Play("Placeholder Eel Light");

            }
           
           
          
        }

        if (lightIsActive)
        {
            if (canPlayLightSource)
            {
                lightSource.Play();
                canPlayLightSource = false;
            }
        }
        else
        {
            lightSource.Stop();
            canPlayLightSource = true;
        }
    }

    void EelAnimations()
    {
        if (grounded)
        {
           
            if (lightIsActive)
            {

            }
            
            else
            {
                animator.Play("Placeholder Eel Idle");

            }
        }
        else if (!grounded)
        {
            animator.Play("Placeholder Eel Fall");
        }
        
    }

    void EelElectricity()
    {
        if (Input.GetButtonDown("Interact") && canAct && grounded && active)
        {
            electricitySource.Play();
            canAct = false;
            StartCoroutine(ElectricityTimer());
            animator.Play("Placeholder Eel Idle");
            animator.Play("Placeholder Eel Electricity");
        }
    }

    void SpawnElectricityObject()
    {
        Instantiate(Electricity, transform.position, transform.rotation);
        StartCoroutine(Flash());
    }

    void ChangeLightSize()
    {
        if (canAct)
        {
            if (lightIsActive)
            {
                currentScaleX += lightScaleSpeedX;
                currentScaleY += lightScaleSpeedY;
                if (currentScaleX > targetLightScaleX)
                    currentScaleX = targetLightScaleX;
                if (currentScaleY > targetLightScaleY)
                    currentScaleY = targetLightScaleY;
            }
            else
            {
                currentScaleX -= lightScaleSpeedX;
                currentScaleY -= lightScaleSpeedY;
                if (currentScaleX < 0.0f)
                    currentScaleX = 0.0f;
                if (currentScaleY < 0.0f)
                    currentScaleY = 0.0f;
            }
        }
        
        LightObj.transform.localScale = new Vector2(currentScaleX, currentScaleY);
    }

    public void MonkeyInteraction(bool pickedUp)
    {
        this.pickedUp = pickedUp;
        GetComponent<SpriteRenderer>().enabled = !pickedUp;
        collider.enabled = !pickedUp;
        if (pickedUp)
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        else
        {
            if (monkey != null && monkey.GetComponent<MonkeyBehavior>().facingRight)
            {
                Vector3 offset = new Vector2(1.5f, -0.5f);
                transform.position = monkey.transform.position + offset;
            }
            else if (monkey != null && !monkey.GetComponent<MonkeyBehavior>().facingRight)
            {
                Vector3 offset = new Vector2(-1.5f, -0.5f);
                transform.position = monkey.transform.position + offset;
            }
            GetComponent<Rigidbody2D>().gravityScale = startGravity;
        }
        if (monkey != null)
            GetComponent<Rigidbody2D>().velocity = new Vector2(monkey.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

    }

    void PickedUp()
    {
        if (monkey != null)
            transform.position = monkey.transform.position + monkeyCarryOffset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
            levelComplete = true;
        if (other.gameObject.tag == "WalkRight")
            runRightCheck = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
            levelComplete = false;
        if (other.gameObject.tag == "WalkRight")
            runRightCheck = false;
    }

    IEnumerator ElectricityTimer()
    {
        yield return new WaitForSeconds(electricityTime);
        canAct = true;
    }

    IEnumerator Flash()
    {
        float flashScaleX = 0.0f, flashScaleY = 0.0f;
        if (!lightIsActive)
        {
            flashScaleX = targetLightScaleX / 2;
            flashScaleY = targetLightScaleY / 2;
        }
        else
        {
            flashScaleX = currentScaleX * 1.1f;
            flashScaleY = currentScaleY * 1.1f;
        }

        for (int i = 0; i < 2; i++)
        {
            currentScaleX = flashScaleX;
            currentScaleY = flashScaleY;
            yield return new WaitForSeconds(flashTime);
            if (!lightIsActive)
            {
                currentScaleX = 0.0f;
                currentScaleY = 0.0f;
            }
            else
            {
                currentScaleX = targetLightScaleX;
                currentScaleY = targetLightScaleY;
            }
            yield return new WaitForSeconds(flashTime);
        }
    }

    IEnumerator LandingTimer()
    {
        yield return new WaitForSeconds(landingTime);
        canAct = true;
    }
}
