using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardLayoutScript : MonoBehaviour
{
    private bool isTablet = false; 
    private float totalVerticalButtonSpacing; 

    public Canvas mainBoard;
    public Canvas eogBoard;
    public GameObject buttonParentObject;
    public GameObject collectionView;

    RectTransform mainBoardRT;
    RectTransform collectionViewRT;
    RectTransform buttonParentRT;

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
        // mainBoardRT.localPosition = new Vector3(mainBoardRT.localPosition.x, 0, 0);

        // var mainBoardAnimator = mainBoard.GetComponent<Animator>();
        // mainBoardAnimator.Play("MainBoardAnimation");

        collectionViewRT = collectionView.GetComponent<RectTransform>();
        buttonParentRT = buttonParentObject.GetComponent<RectTransform>();
        determineIfTablet();
    }

    void determineIfTablet()
    {
        if(mainBoardRT.rect.width < (mainBoardRT.rect.height*1.66))
        {
            isTablet = true;
        }
        getButtonSizes();
    }

    void getButtonSizes()
    {

        if(isTablet)
        {
            //do button sizes for tablet
        }
        else
        {
            boardHeight = mainBoardRT.rect.height;
            cardHeight = (boardHeight*0.9f) / 5;
            cardWidth = (float)(cardHeight * 1.66);
            totalVerticalButtonSpacing = mainBoardRT.rect.height-(cardHeight*5);

            collectionViewRT.sizeDelta = new Vector2(cardWidth * 5, boardHeight);
            buttonParentRT.sizeDelta = new Vector2(cardWidth, cardHeight);
            var image = buttonParentObject.GetComponentInChildren<Image>();
            image.rectTransform.sizeDelta = buttonParentRT.sizeDelta;
        }

        createCardPrefabs();
    }

    void createCardPrefabs()
    {
        buttonParentObject.SetActive(true);
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
            GameObject cardClone = Instantiate(buttonParentObject, new Vector3(0, 0, 0), Quaternion.identity, collectionView.transform);
            var cardCloneRT = cardClone.GetComponent<RectTransform>();
            cardClone.transform.SetParent(collectionView.transform, false);
            cardPositions[n] = cardCloneRT;

            var buttonData = cardClone.GetComponentInChildren<CardFlipHandler>();
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
        float yOrigin = 0-(totalVerticalButtonSpacing/6);
        float verticalSpaceRemaining = collectionViewRT.rect.height;

        foreach (RectTransform card in cardPositions)
        {
            if (verticalSpaceRemaining < cardHeight)
            {
                xOrigin += cardWidth + totalVerticalButtonSpacing/6;
                verticalSpaceRemaining = collectionViewRT.rect.height;

                yOrigin = 0 - (totalVerticalButtonSpacing / 6);

                card.anchoredPosition = new Vector2(xOrigin, yOrigin);

                yOrigin -= (cardHeight + totalVerticalButtonSpacing/6);
                verticalSpaceRemaining -= (cardHeight + totalVerticalButtonSpacing / 6);
                continue;
            }
            card.anchoredPosition = new Vector2(xOrigin, yOrigin);
            yOrigin -= (cardHeight + totalVerticalButtonSpacing / 6);
            verticalSpaceRemaining -= (cardHeight + totalVerticalButtonSpacing / 6);
        }
        buttonParentObject.SetActive(false);
    }
}
