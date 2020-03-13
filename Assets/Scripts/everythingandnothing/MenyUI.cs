using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenyUI : MonoBehaviour
{
    public GameObject pauseMeny;
    private int maxScore = 10;
    public static int scoreCount = 0;
    public static int scene;
    bool paused = false;

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
        Debug.Log(scene + "int");
        SceneManager.LoadScene(scene);

    }

    public void AddScoreCount()
    {
        scoreCount++;
        Debug.Log(scoreCount);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
