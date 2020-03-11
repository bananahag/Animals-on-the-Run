using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdScore : MonoBehaviour
{
    public Text BirdCountText;
    public int birdCountScore = 0;
    void Update()
    {
        BirdCountText.text = birdCountScore.ToString();
    }
}
