using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnHandler : MonoBehaviour
{
    private WSNetworkingClient networkingClient;
    public GameState gameState;
    public TurnIndicatorScript turnIndicator;
    public Image border; 
    public Image timerFill; 
    public Timer timer; 

    private void Awake()
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    public void resetImages()
    {
        border.sprite = Resources.Load<Sprite>("Images/MainBoard/timer_border_blue");
        timerFill.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_blue");
    }

    public void changeTurns()
    {
        if (gameState.currentGameState == CurrentGameState.redTurn || gameState.currentGameState == CurrentGameState.blueTurn)
        {
            switch (gameState.currentGameState)
            {
                case CurrentGameState.redTurn:
                    gameState.currentGameState = CurrentGameState.blueTurn;
                    border.sprite = Resources.Load<Sprite>("Images/MainBoard/timer_border_blue");
                    timerFill.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_blue");
                    break;
                case CurrentGameState.blueTurn:
                    gameState.currentGameState = CurrentGameState.redTurn;
                    border.sprite = Resources.Load<Sprite>("Images/MainBoard/timer_border_red");
                    timerFill.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_red");
                    break;
            }
            turnIndicator.displayTurn(gameState.currentGameState);
            timer.timerStarted = false;
        }
    }

    public void sendTurnChangeToClients()
    {
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
