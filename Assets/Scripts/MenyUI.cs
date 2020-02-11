using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenyUI : MonoBehaviour
{
    public GameObject pauseMeny;
    public static int scene;
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
    public void Resume()
    {
        paused = !paused;
    }
    public void NextLevel()
    {
        if (scene == 3)
        {
            scene = 0;
        }
        scene++;
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
