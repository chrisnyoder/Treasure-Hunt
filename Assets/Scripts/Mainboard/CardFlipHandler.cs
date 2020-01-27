using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CardFlipHandler : MonoBehaviour
{
    public TurnIndicatorScript turnIndicator;
    private EoGScript eoGScript;
    
    [HideInInspector]
    public bool cardAlreadyFlipped;
    private bool cardAlreadySentToClient;
    public GameState gameState;
    private CurrentGameState _previousGameState = CurrentGameState.blueTurn; 
    public EndTurnHandler endTurnHandler;
    public ScoreDisplayHandler scoreDisplay;
    public Timer timer; 

    public CardType cardType;
    public string cardText;

    public GameObject buttonParentObject; 

    private WSNetworkingClient networkingClient;

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
        buttonParentObject.GetComponent<EventTrigger>().enabled = false; 
    }

    public void selectCardOnBoard()
    {
        if (gameState.currentGameState == CurrentGameState.blueTurn || gameState.currentGameState == CurrentGameState.redTurn)
        {
            flipCard();
            changeTurnIfNecessary();
            sendCardSelectionToClients();

            print("sennding this game state: " + gameState.currentGameState);
            networkingClient.sendCurrentGameState(gameState.currentGameState);
        }
    }

    public void flipCard()
    {
        if(!cardAlreadyFlipped)
        {
            cardAlreadyFlipped = true;
            tallyScore();
            playFirstHalfOfCardFlipAnimation();
        }
    }

    private void tallyScore()
    {
        switch (cardType)
        {
            case CardType.blueCard:
                GlobalAudioScript.Instance.playSfxSound("coin_flip");
                gameState.blueTeamScore += 1;
                scoreDisplay.displayScore();
                if (gameState.blueTeamScore >= 8)
                {
                    timer.timerStarted = false;
                    gameState.currentGameState = CurrentGameState.blueWins;
                    StartCoroutine(LaunchEoGAfterDelay());
                }
                break;
            case CardType.redCard:
                GlobalAudioScript.Instance.playSfxSound("correct");
                gameState.redTeamScore += 1;
                scoreDisplay.displayScore();
                if (gameState.redTeamScore >= 7)
                {
                    timer.timerStarted = false;
                    gameState.currentGameState = CurrentGameState.redWins;
                    StartCoroutine(LaunchEoGAfterDelay());
                }
                break;
            case CardType.neutralCard:
                GlobalAudioScript.Instance.playSfxSound("flip_stone");
                break;
            case CardType.shipwreckCard:
                GlobalAudioScript.Instance.playSfxSound("sad_violin");
                timer.timerStarted = false;
                gameState.currentGameState = CurrentGameState.loses;
                StartCoroutine(LaunchEoGAfterDelay());
                break;
        }

        gameState.wordsAlreadySelected.Add(cardText);
    }

    private void changeTurnIfNecessary()
    {
        print("changing turns, if necesary");
        switch (cardType)
        {
            case CardType.blueCard:
                if (gameState.currentGameState == CurrentGameState.redTurn)
                {
                    endTurnHandler.changeTurnTo(CurrentGameState.blueTurn);
                }
                break;
            case CardType.redCard:
                if (gameState.currentGameState == CurrentGameState.blueTurn)
                {
                    endTurnHandler.changeTurnTo(CurrentGameState.redTurn);
                }
                break;
            case CardType.neutralCard:
                switch(gameState.currentGameState)
                {
                    case CurrentGameState.blueTurn:
                        endTurnHandler.changeTurnTo(CurrentGameState.redTurn);
                        break;
                    case CurrentGameState.redTurn:
                        endTurnHandler.changeTurnTo(CurrentGameState.blueTurn);
                        break;
                }
                break;
        }

        sendNewGameStateToClient();
    }

    private void sendCardSelectionToClients()
    {
        if(!cardAlreadySentToClient)
            networkingClient.wordsSelectedQueue.Add(cardText);

        cardAlreadySentToClient = true;
    }

    private void sendNewGameStateToClient()
    {
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }

    private void playFirstHalfOfCardFlipAnimation()
    {
        var rt = gameObject.GetComponent<RectTransform>();
        Sequence s = DOTween.Sequence();

        s.Append(rt.DORotate(new Vector3(0f, 90f, 0), 0.3f, RotateMode.Fast));
        s.Join(rt.DOScale(new Vector3(1.2f, 0.8f, 1), 0.3f));

        s.Play().OnComplete(() =>
        {
            gameObject.GetComponentInChildren<Text>().enabled = false;
            changeCardColor();
        });
    }

    public void changeCardColor()
    {

        switch (cardType)
        {
            case CardType.blueCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/blue_card");
                break;
            case CardType.redCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/red_card"); 
                break;
            case CardType.neutralCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/sand_card"); 
                break;
            case CardType.shipwreckCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/Shipwreck_Card");
                break;
        }

        playSecondHalfOfCardFlipAnimation();
    }

    private void playSecondHalfOfCardFlipAnimation()
    {
        var rt = gameObject.GetComponent<RectTransform>();
        Sequence s = DOTween.Sequence();

        s.Append(rt.DORotate(new Vector3(0f, 180f, 0), 0.3f, RotateMode.Fast));
        s.Join(rt.DOScale(new Vector3(1f, 1f, 1), 0.3f));

        s.Play().OnComplete(() => {
            buttonParentObject.GetComponent<EventTrigger>().enabled = true;
        });
    }

    public void startCardFaceUp()
    {
        switch (cardType)
        {
            case CardType.blueCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/blue_card");
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, -180f, 0f); 
                break;
            case CardType.redCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/red_card");
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, -180, 0)) ;
                break;
            case CardType.neutralCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/sand_card");
                GetComponent<RectTransform>().localRotation = new Quaternion(0, -180, 0, 0);
                break;
            case CardType.shipwreckCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/Shipwreck_Card");
                GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, -180f, 0f);
                break;
        }

        gameObject.GetComponentInChildren<Text>().enabled = false;
        gameObject.GetComponent<Button>().onClick.RemoveListener(flipCard);
        cardAlreadyFlipped = true;
    }

    public void pressToRevealWord()
    {
        var rt = gameObject.GetComponent<RectTransform>();
        Sequence s = DOTween.Sequence();

        s.Append(rt.DORotate(new Vector3(0f, 90f, 0), 0.3f, RotateMode.Fast));
        s.Join(rt.DOScale(new Vector3(1.2f, 0.8f, 1), 0.3f));

        s.Play().OnComplete( () => {

            gameObject.GetComponentInChildren<Text>().enabled = true;
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/beige_card");
            Sequence se = DOTween.Sequence();

            se.Append(rt.DORotate(new Vector3(0f, 0f, 0), 0.3f, RotateMode.Fast));
            se.Join(rt.DOScale(new Vector3(1f, 1f, 1f), 0.3f));
            GlobalAudioScript.Instance.playSfxSound3("pickup2");
        });
    }

    public void unpressToHide()
    {
        print("unpress to reveal event being called");
        playFirstHalfOfCardFlipAnimation();
        GlobalAudioScript.Instance.playSfxSound3("drop");
    }

    IEnumerator LaunchEoGAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameState.LaunchEOGScreen();
    }
}
