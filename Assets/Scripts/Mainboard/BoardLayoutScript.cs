using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardLayoutScript : MonoBehaviour
{
    public Canvas mainBoard;
    public Canvas eogBoard;
    public GameObject buttonObject;
    public GameObject collectionView;

    RectTransform mainBoardRT;
    RectTransform collectionViewRT;
    RectTransform buttonRT;

    float boardHeight;
    float boardWidth;

    int numberOfCards = 25;

    GameState initialGameState;
    CardType[] cardTypes;
    RectTransform[] cardPositions;
    float cardHeight;
    float cardWidth;

    public void receiveGameStateObject(GameState initialGameState)
    {
        this.initialGameState = initialGameState;

        mainBoardRT = mainBoard.GetComponent<RectTransform>();
        mainBoardRT.localPosition = new Vector3(mainBoardRT.localPosition.x, 0, 0);

        collectionViewRT = collectionView.GetComponent<RectTransform>();
        buttonRT = buttonObject.GetComponent<RectTransform>();
        getButtonSizes();
    }

    void getButtonSizes()
    {
        boardHeight = mainBoardRT.rect.height;
        cardHeight = boardHeight / 5;
        cardWidth = (float)(cardHeight * 1.66);

        collectionViewRT.sizeDelta = new Vector2(cardWidth * 5, boardHeight);
        buttonRT.sizeDelta = new Vector2(cardWidth, cardHeight);

        createCardPrefabs();
    }

    void createCardPrefabs()
    {
        buttonObject.SetActive(true);
        if (cardPositions == null)
        {
            cardPositions = new RectTransform[numberOfCards];
        } 
            else
        {
            foreach(RectTransform rt in cardPositions)
            {
                Destroy(rt.gameObject);
            }
        }
    
        for (int n = 0; n < numberOfCards; ++n)
        {
            GameObject cardClone = Instantiate(buttonObject, new Vector3(0, 0, 0), Quaternion.identity, collectionView.transform);
            var cardCloneRT = cardClone.GetComponent<RectTransform>();
            cardClone.transform.SetParent(collectionView.transform, false);
            cardPositions[n] = cardCloneRT;

            var buttonData = cardClone.GetComponent<CardFlipHandler>();
            buttonData.cardType = initialGameState.hiddenBoardList[n].cardType;
            buttonData.cardText = initialGameState.hiddenBoardList[n].labelText;

            var cardText = cardClone.GetComponentInChildren<Text>();
            cardText.text = buttonData.cardText;

            buttonData.gameState = initialGameState;
        }

        layoutCards();
    }

    void layoutCards()
    {
        float xOrigin = 0;
        float yOrigin = 0;
        float verticalSpaceRemaining = collectionViewRT.rect.height;

        foreach (RectTransform card in cardPositions)
        {
            if (verticalSpaceRemaining < cardHeight)
            {
                xOrigin += cardWidth;
                verticalSpaceRemaining = collectionViewRT.rect.height;

                yOrigin = 0;

                card.anchoredPosition = new Vector2(xOrigin, yOrigin);

                yOrigin -= cardHeight;
                verticalSpaceRemaining -= cardHeight;
                continue;
            }
            card.anchoredPosition = new Vector2(xOrigin, yOrigin);
            yOrigin -= cardHeight;
            verticalSpaceRemaining -= cardHeight;
        }
        buttonObject.SetActive(false);
    }
}
