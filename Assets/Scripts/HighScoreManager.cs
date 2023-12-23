using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HighScoreManager : MonoBehaviour
{
    private TextMeshProUGUI highscoreboard;
    private int highScore;


    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.LoseEvent.AddListener(SaveHighScore);
        highscoreboard = GetComponent<TextMeshProUGUI>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreboard.text = String.Format("{0}", highScore);
    }

    private void SaveHighScore(int score)
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }
    }

    private void OnDestroy()
    {
        GameController.instance.LoseEvent.RemoveListener(SaveHighScore);
    }

}
