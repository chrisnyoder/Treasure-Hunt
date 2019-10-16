using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipHandler : MonoBehaviour
{

    private Animator animator;
    private EoGScript eoGScript;
    public GameState gameState;

    public Button card;
    public CardType cardType;
    public string cardImage;
    public string cardText;

    public Sprite blueImage;
    public Sprite redImage;
    public Sprite neutralImage;
    public Sprite shipwreckImage;

    void Start() 
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void FlipCard()
    {
        switch(cardType)
        {
            case CardType.blueCard:
                GlobalAudioScript.Instance.playSfxSound("coin_flip");
                break;
            case CardType.redCard:
                GlobalAudioScript.Instance.playSfxSound("correct");
                break;
            case CardType.neutralCard:
                GlobalAudioScript.Instance.playSfxSound("flip_stone");
                break;
            case CardType.shipwreckCard:
                GlobalAudioScript.Instance.playSfxSound("sad_violin");
                break;
        }

        if (gameState.currentGameState == CurrentGameState.gameInPlay)
        {
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
                gameState.blueTeamScore += 1;
                if (gameState.blueTeamScore >= 8)
                {
                    gameState.currentGameState = CurrentGameState.blueWins;
                    gameState.LaunchEOGScreen();
                }
                break;
            case CardType.redCard:
                card.GetComponent<Image>().sprite = redImage;
                gameState.redTeamScore += 1;
                if (gameState.redTeamScore >= 7)
                {
                    gameState.currentGameState = CurrentGameState.redWins;
                    gameState.LaunchEOGScreen();
                }
                break;
            case CardType.neutralCard:
                card.GetComponent<Image>().sprite = neutralImage;
                break;
            case CardType.shipwreckCard:
                card.GetComponent<Image>().sprite = shipwreckImage;
                gameState.currentGameState = CurrentGameState.loses;
                StartCoroutine(LaunchEoGAfterDelay());
                break;
        }

        animator.enabled = true;
        animator.Play("MainboardButtonAnimationComplete");
    }

    IEnumerator LaunchEoGAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        gameState.LaunchEOGScreen();
    }
}
