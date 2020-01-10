﻿using System.Collections;
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
    public GameState gameState;

    public CardType cardType;
    public string cardText;

    public GameObject buttonParentObject; 

    private WSNetworkingClient networkingClient;

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
        gameObject.GetComponent<Button>().onClick.AddListener(FlipCard);
        buttonParentObject.GetComponent<EventTrigger>().enabled = false; 
    }

    public void FlipCard()
    {
        if(!cardAlreadyFlipped)
        {
            cardAlreadyFlipped = true;
            if (gameState.currentGameState == CurrentGameState.blueTurn || gameState.currentGameState == CurrentGameState.redTurn)
            {
                updateGameState();
                playFirstHalfOfCardFlipAnimation();
            }
        }
    }

    private void updateGameState()
    {  
        switch (cardType)
        {
            case CardType.blueCard:
                GlobalAudioScript.Instance.playSfxSound("coin_flip");
                gameState.blueTeamScore += 1;
                if (gameState.blueTeamScore >= 8)
                {
                    gameState.currentGameState = CurrentGameState.blueWins;
                    StartCoroutine(LaunchEoGAfterDelay());
                } else if(gameState.currentGameState == CurrentGameState.redTurn) 
                {
                    turnIndicator.changeTurns();
                }
                break;
            case CardType.redCard:
                GlobalAudioScript.Instance.playSfxSound("correct");
                gameState.redTeamScore += 1;
                if (gameState.redTeamScore >= 7)
                {
                    gameState.currentGameState = CurrentGameState.redWins;
                    StartCoroutine(LaunchEoGAfterDelay());
                } else if(gameState.currentGameState == CurrentGameState.blueTurn) 
                {
                    turnIndicator.changeTurns();
                }
                break;
            case CardType.neutralCard:
                GlobalAudioScript.Instance.playSfxSound("flip_stone");
                turnIndicator.changeTurns();
                break;
            case CardType.shipwreckCard:
                GlobalAudioScript.Instance.playSfxSound("sad_violin");
                gameState.currentGameState = CurrentGameState.loses;
                StartCoroutine(LaunchEoGAfterDelay());
                break;
        }

        gameObject.GetComponent<Button>().onClick.RemoveListener(FlipCard);

        gameState.wordsAlreadySelected.Add(cardText);
        updateClients();
    }

    private void updateClients()
    {
        networkingClient.wordsSelectedQueue.Add(cardText);
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
        gameObject.GetComponent<Button>().onClick.RemoveListener(FlipCard);
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
        });
    }

    public void unpressToHide()
    {
        print("unpress to reveal event being called");
        playFirstHalfOfCardFlipAnimation();
    }

    IEnumerator LaunchEoGAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameState.LaunchEOGScreen();
    }
}
