using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnIndicatorScript : MonoBehaviour
{    
    public Image turnChangeImage;
    public Text turnIndicatorText; 
    public Timer timer; 
    public Image timerImage; 
    public EndTurnHandler endTurn; 
  
    private bool gameHasStarted = false; 
    private CurrentGameState _currentGameState; 

    private void Start() 
    {
        turnChangeImage.gameObject.SetActive(false);
        turnIndicatorText.gameObject.SetActive(false);
    }

    public void displayTurn(CurrentGameState currentGameState)
    {
        gameObject.SetActive(true);

        if(_currentGameState == currentGameState)
        {
            _currentGameState = currentGameState;
            return;
        }

        _currentGameState = currentGameState;

        displayTurnIndicators();

        gameObject.SetActive(true);
        turnChangeImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        turnChangeImage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1) * 2.1f;
      
        GetComponent<Image>().DOFade(0.5f, 1.0f).SetDelay(0.7f).Play();
        turnChangeImage.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 1.0f).SetDelay(1.2f).OnPlay(() => {
            
            GlobalAudioScript.Instance.playSfxSound("Slam_05");
            
            turnChangeImage.gameObject.SetActive(true);
            turnIndicatorText.gameObject.SetActive(true);

        }).SetEase(Ease.OutBounce).Play().OnComplete(() => {

            GetComponent<Image>().DOFade(0.0f, 1.0f).Play();
            turnChangeImage.GetComponent<RectTransform>().DOAnchorPosX(3000, 0.7f, false).SetDelay(0.7f).Play().OnComplete(() => {
                gameObject.SetActive(false);

                turnChangeImage.gameObject.SetActive(false);
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
                GetComponent<Image>().color = new Color32(0, 171, 184, 0);
                turnChangeImage.sprite = Resources.Load<Sprite>("Images/MainBoard/blue_bg");
                turnIndicatorText.text = LocalizationManager.instance.GetLocalizedText("blue_teams_turn");
                break;
            case CurrentGameState.redTurn:
                GetComponent<Image>().color = new Color32(138, 18, 46, 0);
                turnChangeImage.sprite = Resources.Load<Sprite>("Images/MainBoard/red_bg");
                turnIndicatorText.text = LocalizationManager.instance.GetLocalizedText("red_teams_turn");
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
