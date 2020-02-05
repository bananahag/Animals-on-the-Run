using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenyUI : MonoBehaviour
{
    public GameObject pauseMeny;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
        if (paused)
        {
            Time.timeScale = 0;
            pauseMeny.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMeny.SetActive(false);
        }
    }
}
