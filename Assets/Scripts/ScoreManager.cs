using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    private int score;
    private TextMeshProUGUI scoreboard;
    
    public GameObject gameOverScreen;


    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.UpdateScoreEvent.AddListener(UpdateScore);
        GameController.instance.GameOverEvent.AddListener(ShowGameOverScreen);
        GameController.instance.playerInputActions.GameOverScreen.MousePrimaryClick.started += ToggleGameOverScreen;
        scoreboard = GetComponent<TextMeshProUGUI>();
        scoreboard.text = "0";
        HideGameOverScreen();
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        scoreboard.text = String.Format("{0}", newScore);
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
        GameController.instance.playerInputActions.GameOverScreen.Disable();
    }

    public void ShowGameOverScreen(int score)
    {
        gameOverScreen.SetActive(true);
        GameController.instance.playerInputActions.GameOverScreen.Enable();
    }

    public void ToggleGameOverScreen(InputAction.CallbackContext obj)
    {
        if (gameOverScreen.activeInHierarchy)
        {
            gameOverScreen.SetActive(false);
        } else
        {
            gameOverScreen.SetActive(true);
        }
        
    }

    private void OnDestroy()
    {
        GameController.instance.UpdateScoreEvent.RemoveListener(UpdateScore);
        GameController.instance.GameOverEvent.RemoveListener(ShowGameOverScreen);
    }
}
