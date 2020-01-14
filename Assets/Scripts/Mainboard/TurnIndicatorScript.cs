﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnIndicatorScript : MonoBehaviour
{    
    public Image turnIndicatorBackground; 
    public Text turnIndicatorText; 

    public void displayTurn(CurrentGameState currentGameState)
    {
        turnIndicatorBackground.gameObject.SetActive(true);
        turnIndicatorText.gameObject.SetActive(true);

        switch (currentGameState)
        {
            case CurrentGameState.blueTurn:
                turnIndicatorText.text = "Blue team's turn";
                turnIndicatorBackground.color = new Color32(0, 171, 184, 0);
                break;
            case CurrentGameState.redTurn:
                turnIndicatorText.text = "Red team's turn";
                turnIndicatorBackground.color = new Color32(138, 18, 46, 0);
                break;
        }

        turnIndicatorBackground.DOFade(0.313f, 1f).SetDelay(1).OnComplete(() => {
            turnIndicatorBackground.DOFade(0, 1f).SetDelay(0.5f).OnComplete(() => {
                turnIndicatorBackground.gameObject.SetActive(false);
            });
        });

        turnIndicatorText.DOFade(1f, 1f).SetDelay(1).OnComplete(() => {
            turnIndicatorText.DOFade(0, 1f).SetDelay(0.5f).OnComplete(() => {
                turnIndicatorText.gameObject.SetActive(false);
            });
        });
    } 
}