using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI scoreboard;
    
    public GameObject loseScreen;


    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.UpdateScoreEvent.AddListener(UpdateScore);
        GameController.instance.LoseEvent.AddListener(ShowLoseScreen);
        scoreboard = GetComponent<TextMeshProUGUI>();
        scoreboard.text = "0";
        HideLoseScreen();
    }

    public void UpdateScore(int newScore)
    {
        scoreboard.text = String.Format("{0}", newScore);
    }

    public void HideLoseScreen()
    {
        loseScreen.SetActive(false);
    }

    public void ShowLoseScreen(int score)
    {
        loseScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        GameController.instance.UpdateScoreEvent.RemoveListener(UpdateScore);
        GameController.instance.LoseEvent.RemoveListener(ShowLoseScreen);
    }
}
