using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI scoreboard;
    
    public GameObject gameOverScreen; // TODO: Set lose screen


    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.UpdateScoreEvent.AddListener(UpdateScore);
        GameController.instance.LoseEvent.AddListener(ShowGameOverScreen);
        scoreboard = GetComponent<TextMeshProUGUI>();
        scoreboard.text = "0";
        HideGameOverScreen();
    }

    public void UpdateScore(int newScore)
    {
        scoreboard.text = String.Format("{0}", newScore);
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }

    public void ShowGameOverScreen(int score)
    {
        gameOverScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        GameController.instance.UpdateScoreEvent.RemoveListener(UpdateScore);
        GameController.instance.LoseEvent.RemoveListener(ShowGameOverScreen);
    }
}
