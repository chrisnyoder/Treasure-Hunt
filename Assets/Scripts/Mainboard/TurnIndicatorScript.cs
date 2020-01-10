using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnIndicatorScript : MonoBehaviour
{
    private GameState _gameState;
    
    public WSNetworkingClient networkingClient; 
    public Image turnIndicatorBackground; 
    public Text turnIndicatorText; 

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    public void displayTurn(GameState gameState)
    {
        _gameState = gameState;
        var currentGameState = gameState.currentGameState;

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

        turnIndicatorBackground.DOFade(0.313f, 1f).OnComplete(() => {
            turnIndicatorBackground.DOFade(0, 1f).OnComplete(() => {
                turnIndicatorBackground.gameObject.SetActive(false);
            });
        });

        turnIndicatorText.DOFade(1f, 1f).OnComplete(() => {
            turnIndicatorText.DOFade(0, 1f).OnComplete(() => {
                turnIndicatorText.gameObject.SetActive(false);
            });
        });
    } 

    public void changeTurns()
    {
        if(_gameState.currentGameState == CurrentGameState.redTurn || _gameState.currentGameState == CurrentGameState.blueTurn)
        {
            switch (_gameState.currentGameState)
            {
                case CurrentGameState.redTurn:
                    _gameState.currentGameState = CurrentGameState.blueTurn;
                    break;
                case CurrentGameState.blueTurn:
                    _gameState.currentGameState = CurrentGameState.redTurn;
                    break;
            }

            displayTurn(_gameState);
            networkingClient.sendCurrentGameState(_gameState.currentGameState);
        }
    }
}
