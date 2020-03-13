using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScale : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject OtherScale;
    Vector3 oldpos;
    Vector3 nextpos;
    Vector3 startpos;
    public int amountBoxes;
    public float length;
    public float travelTime;
    private float fraction;
    bool scalehit;
    public AudioSource scaleMoveAudioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startpos = transform.position;
        oldpos = transform.position;
        nextpos = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        
        if (transform.position != nextpos)
        {
            
            fraction += Time.deltaTime / travelTime;
            rb.MovePosition(Vector2.Lerp(oldpos, nextpos, fraction));

        }
        else 
        {
        scaleMoveAudioSource.Stop();
        }
       
    }
  

    public void MoveScaleDown()
    {
        scaleMoveAudioSource.Play();
        amountBoxes++;
        Vector3 offset = new Vector3(0.0f, amountBoxes * -length);
        nextpos = startpos + offset;
        oldpos = transform.position;
        fraction = 0.0f;
    }
    public void MoveScaleUp()
    {
        scaleMoveAudioSource.Play();
        amountBoxes--;
        Vector3 offset = new Vector3(0.0f, amountBoxes * -length);
        nextpos = startpos + offset;
        oldpos = transform.position;
        fraction = 0.0f;
    }

}
