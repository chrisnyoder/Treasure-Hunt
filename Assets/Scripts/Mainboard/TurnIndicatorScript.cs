using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnIndicatorScript : MonoBehaviour
{    
    public Image turnIndicatorBackground; 
    public Text turnIndicatorText; 
    public Timer timer; 
    public Image timerImage; 
    public EndTurnHandler endTurn; 
    private bool gameHasStarted = false; 
    private CurrentGameState _currentGameState; 

    private void Start() 
    {
        turnIndicatorBackground.gameObject.SetActive(false);
        turnIndicatorText.gameObject.SetActive(false);
    }

    public void displayTurn(CurrentGameState currentGameState)
    {
        _currentGameState = currentGameState;

        turnIndicatorBackground.gameObject.SetActive(true);
        turnIndicatorText.gameObject.SetActive(true);

        displayTurnIndicators();

        turnIndicatorBackground.DOFade(0.313f, 1f).SetDelay(1).OnComplete(() => {
            turnIndicatorBackground.DOFade(0, 1f).SetDelay(0.5f).OnComplete(() => {
                turnIndicatorBackground.gameObject.SetActive(false);
                timer.timerStarted = true;
                timer.resetTimer();
                changeTimerBarColor();
            });
        });

        turnIndicatorText.DOFade(1f, 1f).SetDelay(1).OnComplete(() => {
            turnIndicatorText.DOFade(0, 1f).SetDelay(0.5f).OnComplete(() => {
                turnIndicatorText.gameObject.SetActive(false);
                timer.timerStarted = true;
                timer.resetTimer();
                changeTimerBarColor();
            });
        });
    } 

    private void displayTurnIndicators()
    {
        switch (_currentGameState)
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
    }

    private void changeTimerBarColor()
    {
        switch (_currentGameState)
        {
            case CurrentGameState.blueTurn:
                timerImage.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_blue");
                break;
            case CurrentGameState.redTurn:
                timerImage.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_red");
                break;
        }
    }

    public void sendInitialGameStateToServer()
    {
        if (!gameHasStarted)
        {
            timer.networkingClient.sendCurrentGameState(CurrentGameState.blueTurn);
            gameHasStarted = true;
            if(GlobalDefaults.Instance.tutorialIsOn)
            {
                timer.toggleTimer();
            }
        }
    }
}
