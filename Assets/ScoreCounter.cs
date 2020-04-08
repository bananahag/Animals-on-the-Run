using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreCounter : MonoBehaviour
{
  
    [Tooltip("The max amount of birds to rescue.")]
    public int maxScorePerLevel = 0;
    private int scoreCount = 0;
    Text scoreText;
    // Start is called before the first frame update

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }
    void Start()
    {
        if (scoreText != null)
            scoreText.text = scoreCount.ToString() + "/" + maxScorePerLevel;
        
    }
    public void AddScoreCount()
    {
        scoreCount++;
        PrintScore();
    }

    void PrintScore()
    {
        scoreText.text = scoreCount.ToString() + "/" + maxScorePerLevel;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
