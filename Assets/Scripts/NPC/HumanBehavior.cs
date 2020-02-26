using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehavior : MonoBehaviour
{

    [HideInInspector]
    public Animator an = null;
    [HideInInspector]
    public SpriteRenderer sr = null;
    [HideInInspector]
    public Rigidbody2D rb = null;
    HumanState currentState = null;


    private void Awake()
    {
        an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;

        currentState.Enter();
    }
    public void OnValidate()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();  
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void ChangeState(HumanState targetState)
    {
        currentState.Exit();
        currentState = targetState;
        currentState.Enter();
    }
}
