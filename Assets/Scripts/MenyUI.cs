using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenyUI : MonoBehaviour
{
    public GameObject pauseMeny;
    SwapCharacter player;
    public static int scene;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        if (scene != 0)
        {
        player = GameObject.Find("Player").GetComponent<SwapCharacter>();

        }
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

        if (scene != 0)
        {
            if (player.levelcompleted)
            {
                paused = true;
                NextLevel();
            }
        }
    }

    public void NextLevel()
    {
        if (scene == 3)
        {
            scene = 0;
        }
        Debug.Log(scene + "int");
        scene++;
            SceneManager.LoadScene(scene);
       
        
    }

    public void QuitGame()
    {
        Application.Quit(1);
    }
}
