using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnHandler : MonoBehaviour
{

    private WSNetworkingClient networkingClient;
    public GameState gameState;
    public TurnIndicatorScript turnIndicator;

    private void Awake()
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    public void changeTurns()
    {
        if (gameState.currentGameState == CurrentGameState.redTurn || gameState.currentGameState == CurrentGameState.blueTurn)
        {
            switch (gameState.currentGameState)
            {
                case CurrentGameState.redTurn:
                    gameState.currentGameState = CurrentGameState.blueTurn;
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/blue_timer@2x");
                    break;
                case CurrentGameState.blueTurn:
                    gameState.currentGameState = CurrentGameState.redTurn;
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/red_timer@2x");
                    break;
            }
            turnIndicator.displayTurn(gameState.currentGameState);
        }
    }

    public void setTurnTo(CurrentGameState turn)
    {

    }

    public void sendTurnChangeToClients()
    {
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
