﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipHandler : MonoBehaviour
{
    private Animator animator;
    private EoGScript eoGScript;
    public bool cardIsFlipped;
    public GameState gameState;

    public Button card;
    public CardType cardType;
    public string cardImage;
    public string cardText;
    private Text textField;

    public Sprite blueImage;
    public Sprite redImage;
    public Sprite neutralImage;
    public Sprite shipwreckImage;

    private WSNetworkingClient networkingClient;

    private void Awake() 
    {
        animator = gameObject.GetComponent<Animator>();
        GetComponent<FloatAnimation>().enabled = false;
        textField = card.GetComponentInChildren<Text>();

        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    public void FlipCard()
    {
        cardIsFlipped = true;

        if (gameState.currentGameState == CurrentGameState.gameInPlay)
        {
            switch (cardType)
            {
                case CardType.blueCard:
                    GlobalAudioScript.Instance.playSfxSound("coin_flip");
                    GetComponent<FloatAnimation>().enabled = true;
                    gameState.blueTeamScore += 1;
                    if (gameState.blueTeamScore >= 8)
                    {
                        gameState.currentGameState = CurrentGameState.blueWins;
                    }
                    break;
                case CardType.redCard:
                    GlobalAudioScript.Instance.playSfxSound("correct");
                    GetComponent<FloatAnimation>().enabled = true;
                    gameState.redTeamScore += 1;
                    if (gameState.redTeamScore >= 7)
                    {
                        gameState.currentGameState = CurrentGameState.redWins;
                    }
                    break;
                case CardType.neutralCard:
                    GlobalAudioScript.Instance.playSfxSound("flip_stone");
                    break;
                case CardType.shipwreckCard:
                    GlobalAudioScript.Instance.playSfxSound("sad_violin");
                    gameState.currentGameState = CurrentGameState.loses;
                    break;
            }

            Destroy(textField);
            card.interactable = false;
            animator.Play("MainboardButtonAnimation");
        }
    }

    public void changeCardColor()
    {
        animator.enabled = false;

        switch (cardType)
        {
            case CardType.blueCard:
                card.GetComponent<Image>().sprite = blueImage;
                break;
            case CardType.redCard:
                card.GetComponent<Image>().sprite = redImage;
                break;
            case CardType.neutralCard:
                card.GetComponent<Image>().sprite = neutralImage;
                break;
            case CardType.shipwreckCard:
                card.GetComponent<Image>().sprite = shipwreckImage;
                break;
        }

        if(gameState.currentGameState != CurrentGameState.gameInPlay)
        {
            StartCoroutine(LaunchEoGAfterDelay());
        }

        gameState.wordsSelected.Add(cardText);
        networkingClient.sendWordsSelected(gameState.wordsSelected);

        animator.enabled = true;
        animator.Play("MainboardButtonAnimationComplete");
    }

    public void startCardFaceUp()
    {
        animator.enabled = false;
        switch (cardType)
        {
            case CardType.blueCard:
                card.GetComponent<Image>().sprite = blueImage;
                GetComponent<FloatAnimation>().enabled = true;
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, -180f, 0f); 
                break;
            case CardType.redCard:
                card.GetComponent<Image>().sprite = redImage;
                GetComponent<FloatAnimation>().enabled = true;
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, -180, 0)) ;
                break;
            case CardType.neutralCard:
                card.GetComponent<Image>().sprite = neutralImage;
                GetComponent<RectTransform>().localRotation = new Quaternion(0, -180, 0, 0);
                break;
            case CardType.shipwreckCard:
                card.GetComponent<Image>().sprite = shipwreckImage;
                GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, -180f, 0f);
                break;
        }
        
        Destroy(textField);
        card.interactable = false;
        cardIsFlipped = true;
    }

    IEnumerator LaunchEoGAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameState.LaunchEOGScreen();
        print("current game state is: " + gameState.currentGameState);
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
