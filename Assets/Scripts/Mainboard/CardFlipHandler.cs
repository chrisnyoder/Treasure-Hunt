using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipHandler : MonoBehaviour
{

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
            card.interactable = false;
            switch (cardType)
            {
                case CardType.blueCard:
                    card.GetComponentInChildren<Text>().text = "Blue Card";
                    break;
                case CardType.redCard:
                    card.GetComponentInChildren<Text>().text = "Red Card";
                    break;
                case CardType.neutralCard:
                    card.GetComponentInChildren<Text>().text = "Neutral Card";
                    break;
                case CardType.shipwreckCard:
                    card.GetComponentInChildren<Text>().text = "Shipwreck Card";
                    break;
            }
        }
    }
}
