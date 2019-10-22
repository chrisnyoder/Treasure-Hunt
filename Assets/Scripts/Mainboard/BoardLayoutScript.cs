using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardLayoutScript : MonoBehaviour
{
    private bool isTablet = false; 
    private float totalSpacing; 

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
            print("is tablet");
            boardWidth = mainBoardRT.rect.width;
            cardWidth = (boardWidth * 0.9f) / 5;
            cardHeight = (float)(cardWidth*0.6);
            totalSpacing = mainBoardRT.rect.width - (cardWidth * 5);

            collectionViewRT.sizeDelta = new Vector2(boardWidth, boardWidth*0.6f);
        }
        else
        {
            print("is phone");
            boardHeight = mainBoardRT.rect.height;
            cardHeight = (boardHeight*0.9f) / 5;
            cardWidth = (float)(cardHeight * 1.66);
            totalSpacing = mainBoardRT.rect.height-(cardHeight*5);

            collectionViewRT.sizeDelta = new Vector2((cardWidth * 5)+totalSpacing, boardHeight);

        }

        buttonParentRT.sizeDelta = new Vector2(cardWidth, cardHeight);
        var image = buttonParentObject.GetComponentInChildren<Image>();
        image.rectTransform.sizeDelta = buttonParentRT.sizeDelta;

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
        if(isTablet)
        {
            float xOrigin = 0 + (totalSpacing / 6); 
            float yOrigin = 0;
            float rowNumber = 1;

            foreach (RectTransform card in cardPositions)
            {
                if (rowNumber > 5)
                {
                    xOrigin += cardWidth + totalSpacing / 6;
                    rowNumber = 1;

                    yOrigin = 0;

                    card.anchoredPosition = new Vector2(xOrigin, yOrigin);

                    yOrigin -= (cardHeight + totalSpacing / 6);
                    rowNumber += 1;
                    continue;
                }
                card.anchoredPosition = new Vector2(xOrigin, yOrigin);
                yOrigin -= (cardHeight + totalSpacing / 6);
                rowNumber += 1;
            }
            buttonParentObject.SetActive(false);
        }
        else
        {
            float xOrigin = 0;
            float yOrigin = 0 - (totalSpacing / 6);
            float verticalSpaceRemaining = collectionViewRT.rect.height;

            foreach (RectTransform card in cardPositions)
            {
                if (verticalSpaceRemaining < cardHeight)
                {
                    xOrigin += cardWidth + totalSpacing / 6;
                    verticalSpaceRemaining = collectionViewRT.rect.height;

                    yOrigin = 0 - (totalSpacing / 6);

                    card.anchoredPosition = new Vector2(xOrigin, yOrigin);

                    yOrigin -= (cardHeight + totalSpacing / 6);
                    verticalSpaceRemaining -= (cardHeight + totalSpacing / 6);
                    continue;
                }
                card.anchoredPosition = new Vector2(xOrigin, yOrigin);
                yOrigin -= (cardHeight + totalSpacing / 6);
                verticalSpaceRemaining -= (cardHeight + totalSpacing / 6);
            }
            buttonParentObject.SetActive(false);
        }
    }
}
