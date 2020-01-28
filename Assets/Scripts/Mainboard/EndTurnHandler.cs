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
        timerFill.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_blue");
    }

    public void toggleTurns()
    {
        switch (gameState.currentGameState)
        {
            case CurrentGameState.redTurn:
                changeTurnTo(CurrentGameState.blueTurn);
                break;
            case CurrentGameState.blueTurn:
                changeTurnTo(CurrentGameState.redTurn);
                break;
            default:
                changeTurnTo(CurrentGameState.blueTurn);
                break;
        }
        sendTurnChangeToClients();
    }

    public void changeTurnTo(CurrentGameState newGameState)
    {
        timer.timerStarted = false;
        turnIndicator.displayTurn(newGameState);
        gameState.currentGameState = newGameState;
    }

    public void sendTurnChangeToClients()
    {
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
