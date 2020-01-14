using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    print("was red turn, turning to blue");
                    gameState.currentGameState = CurrentGameState.blueTurn;
                    break;
                case CurrentGameState.blueTurn:
                    print("was blue turn, turning to red");
                    gameState.currentGameState = CurrentGameState.redTurn;
                    break;
            }
            turnIndicator.displayTurn(gameState.currentGameState);
        }
    }

    public void sendTurnChangeToClients()
    {
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
