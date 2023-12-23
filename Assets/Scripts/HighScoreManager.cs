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
        GameController.instance.GameOverEvent.AddListener(SaveHighScore);
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
            PlayerPrefs.Save();
        }
    }

    public void ClearHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
        highscoreboard.text = String.Format("{0}", 0);
    }

    private void OnDestroy()
    {
        GameController.instance.GameOverEvent.RemoveListener(SaveHighScore);
    }

}
