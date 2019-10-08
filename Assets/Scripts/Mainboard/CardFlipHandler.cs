using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipHandler : MonoBehaviour
{

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

    public void FlipCard()
    {
        if (gameState.currentGameState == CurrentGameState.gameInPlay)
        {
            var txt = card.GetComponentInChildren<Text>();
            Destroy(txt);
            card.interactable = false;
            switch (cardType)
            {
                case CardType.blueCard:

                    print("blue card clicked");
                    card.GetComponent<Image>().sprite = blueImage;
                    gameState.blueTeamScore += 1;
                    if (gameState.blueTeamScore >= 8)
                    {
                        gameState.currentGameState = CurrentGameState.blueWins;
                        gameState.LaunchEOGScreen();
                    }
                    break;
                case CardType.redCard:
                    print("red card clicked");
                    card.GetComponent<Image>().sprite = redImage;
                    gameState.redTeamScore += 1;
                    if (gameState.redTeamScore >= 7)
                    {
                        gameState.currentGameState = CurrentGameState.redWins;
                        gameState.LaunchEOGScreen();
                    }
                    break;
                case CardType.neutralCard:
                    print("neutral card clicked");
                    card.GetComponent<Image>().sprite = neutralImage;
                    break;
                case CardType.shipwreckCard:
                    card.GetComponent<Image>().sprite = shipwreckImage;
                    gameState.currentGameState = CurrentGameState.loses;
                    gameState.LaunchEOGScreen();
                    break;
            }
        }
    }

}
