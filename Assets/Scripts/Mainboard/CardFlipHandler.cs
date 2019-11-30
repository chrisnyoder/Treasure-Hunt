using System.Collections;
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

    public Sprite blueImage;
    public Sprite redImage;
    public Sprite neutralImage;
    public Sprite shipwreckImage;

    public WSNetworkingClient networkingClient;

    void Start() 
    {
        animator = gameObject.GetComponent<Animator>();
        GetComponent<FloatAnimation>().enabled = false;
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

            var txt = card.GetComponentInChildren<Text>();
            Destroy(txt);
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

    IEnumerator LaunchEoGAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameState.LaunchEOGScreen();
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
