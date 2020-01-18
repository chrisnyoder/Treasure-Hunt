﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public RectTransform parent; 
    public RectTransform mask;
    public bool timerStarted = false; 
    public EndTurnHandler endTurn; 

    public MainBoardNetworkingClient networkingClient;

    public float secondsElapsed = 0; 

    private void Start() 
    {
        resetTimer();
    }

    public void resetTimer()
    {
        secondsElapsed = 0;
    }

    public void toggleTimer()
    {
        networkingClient.pauseGame(pauseGameCallback);
    }

    private void pauseGameCallback(JSONObject data)
    {
        if(timerStarted)
        {
            timerStarted = false;
        } else {
            timerStarted = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (networkingClient.timerObject != null)
        {
            if (Mathf.Abs((float.Parse(networkingClient.timerObject.timeTakenOnTurn) - secondsElapsed)) > 5 && timerStarted)
            {
                secondsElapsed = float.Parse(networkingClient.timerObject.timeTakenOnTurn);
            }
        };

        if(timerStarted && secondsElapsed <= 30)
        {
            secondsElapsed += Time.deltaTime;
            float percentTimeLeft = secondsElapsed / 30;
            mask.offsetMin = new Vector2(0, parent.sizeDelta.y * percentTimeLeft);
        } else if(timerStarted && secondsElapsed >= 30) 
        {
            timerStarted = false;
            resetTimer();
            networkingClient.timerObject.timeTakenOnTurn = "0";
            endTurn.changeTurns();
            endTurn.sendTurnChangeToClients();
        }
    }
}
