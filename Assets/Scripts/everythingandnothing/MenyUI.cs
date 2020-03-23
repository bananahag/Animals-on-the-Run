﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenyUI : MonoBehaviour
{
    public GameObject pauseMeny;
    public Text scoreText;
    [Tooltip("The max amount of birds to rescue.")]
    public int maxScore = 10;
    public static int scoreCount = 0;
    public static int scene;
    bool paused = false;
    public AudioSource buttonAudioSource;
    public AudioSource hoveroverSound;
   
    private void Start()
    {
        if (scoreText != null)
            scoreText.text = scoreCount.ToString() + "/" + maxScore;
    }
    void Update()
    {

        if (scene != 0)
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

    public void Resume()
    {
        paused = !paused;
    }
    
    public void NextLevel()
    {
        
        scene++;
        SceneManager.LoadScene(scene);
        
    }

    public void AddScoreCount()
    {
        scoreCount++;
        PrintScore();
    }

    void PrintScore()
    {
        scoreText.text = scoreCount.ToString() + "/" + maxScore;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void HoveroverSound()
    {
        hoveroverSound.Play();
    }

    public void OnClickSound()
    {
        buttonAudioSource.Play();
    }
}
