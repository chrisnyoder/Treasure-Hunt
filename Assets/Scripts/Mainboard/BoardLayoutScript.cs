﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoardLayoutScript : MonoBehaviour
{
    private bool isTablet = false; 
    private float totalSpacing; 

    public CodeTabScript codeTabScript;
    public Canvas mainBoard;
    public Canvas eogBoard;
    public TurnIndicatorScript turnIndicator; 
    public GameObject buttonParentObject;
    public GameObject collectionView;
    
    RectTransform mainBoardRT;
    RectTransform collectionViewRT;
    RectTransform buttonParentRT;
    public RectTransform codeDisplayRT; 
    public RectTransform menuButtonRt; 
    public RectTransform endTurnButtonRT; 
    public RectTransform menuParentRT; 

    public WSNetworkingClient networkingClient;
    public EndTurnHandler endTurnHandler;
    public ScoreDisplayHandler scoreDisplay;

    float boardHeight;
    float boardWidth;

    int numberOfCards = 25;

    GameState _initialGameState;
    CardType[] cardTypes;
    RectTransform[] cardPositions;
    float cardHeight;
    float cardWidth;

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    private void Start() 
    {
        if(GlobalDefaults.Instance.isTablet)
        {
            var backgroundImage = this.GetComponent<Image>();
            var tabletBackgroundImage = Resources.Load<Sprite>("Images/Backgrounds/iPad_12_MB_Background");
            resizeMenuButton();
            moveEndTurnButton();
            rotateEndTurnButton();

            if(tabletBackgroundImage != null && backgroundImage != null)
            {
                backgroundImage.sprite = tabletBackgroundImage;
            }
        }    
    }

    public void receiveGameStateObject(GameState initialGameState)
    {
        this._initialGameState = initialGameState;

        mainBoardRT = mainBoard.GetComponent<RectTransform>();
        
        collectionViewRT = collectionView.GetComponent<RectTransform>();
        buttonParentRT = buttonParentObject.GetComponent<RectTransform>();
        endTurnHandler.gameState = initialGameState;
        scoreDisplay.receiveInitialGameState(initialGameState);
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
            cardHeight = (float)(cardWidth*0.6316);
            totalSpacing = mainBoardRT.rect.width - (cardWidth * 5) - 100;

            collectionViewRT.sizeDelta = new Vector2((boardWidth-totalSpacing), (boardWidth-totalSpacing)*0.6316f);
        }
        else
        {
            print("is phone");
            boardHeight = mainBoardRT.rect.height;
            cardHeight = (boardHeight*0.9f) / 5;
            cardWidth = (float)(cardHeight * 1.583);
            totalSpacing = mainBoardRT.rect.height-(cardHeight*5);

            collectionViewRT.sizeDelta = new Vector2((cardWidth * 5)+totalSpacing, boardHeight);

        }

        buttonParentRT.sizeDelta = new Vector2(cardWidth, cardHeight);

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
            buttonData.cardType = _initialGameState.hiddenBoardList[n].cardType;
            buttonData.cardText = _initialGameState.hiddenBoardList[n].labelText;

            var cardText = cardClone.GetComponentInChildren<Text>();
            cardText.text = buttonData.cardText;

            buttonData.gameState = _initialGameState;

            if(_initialGameState.wordsAlreadySelected.Contains(cardText.text))
            {
                buttonData.startCardFaceUp();
            }
        }

        layoutCards();
    }

    void layoutCards()
    {
        if(isTablet)
        {
            print("total spacing is: " + totalSpacing);
            var vertSpacing = collectionViewRT.sizeDelta.y - (buttonParentRT.sizeDelta.y*5);
            var horSpacing = collectionViewRT.sizeDelta.x - (buttonParentRT.sizeDelta.x*5);
            float xOrigin = 0 + (horSpacing / 6); 
            float yOrigin = 0 + (-vertSpacing / 6);
            float rowNumber = 1;

            foreach (RectTransform card in cardPositions)
            {
                if (rowNumber > 5)
                {
                    xOrigin += cardWidth + horSpacing / 6;
                    rowNumber = 1;

                    yOrigin = 0 + (-totalSpacing / 6);

                    card.anchoredPosition = new Vector2(xOrigin, yOrigin);

                    yOrigin -= (cardHeight + vertSpacing / 6);
                    rowNumber += 1;
                    continue;
                }
                card.anchoredPosition = new Vector2(xOrigin, yOrigin);
                yOrigin -= (cardHeight + vertSpacing / 6);
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

    void resizeMenuButton()
    {
        menuParentRT.localPosition = new Vector2(menuParentRT.localPosition.x - 30, menuParentRT.localPosition.y);
        menuButtonRt.sizeDelta = new Vector2(menuButtonRt.sizeDelta.x, 86);
    }

    void moveEndTurnButton()
    {
        endTurnButtonRT.anchorMin = new Vector2(0.5f, 0);
        endTurnButtonRT.anchorMax = new Vector2(0.5f, 0);
        endTurnButtonRT.anchoredPosition = new Vector2(0, 0);
    }

    void rotateEndTurnButton()
    {
        endTurnButtonRT.transform.Rotate(0, 0, 90f);
    }

    public void dismissCodeDisplay()
    {
        if(codeDisplayRT != null) 
        {
            codeDisplayRT.DOAnchorPosY(-2000, 1f, false);
            codeDisplayRT.GetComponent<Image>().DOFade(0, 0.3f);
        }

        turnIndicator.displayTurn(_initialGameState.currentGameState);
        networkingClient.sendCurrentGameState(_initialGameState.currentGameState);
    }

    public void runMainBoardAnimation()
    {
        print("run main board function being called");
        var codeDisplayBackground = GameObject.Find("CodeDisplayBackground");
        var mainBoardRT = GameObject.Find("MainBoardCanvas").GetComponent<RectTransform>();
        
        if(codeDisplayBackground != null)
            codeDisplayBackground.GetComponentInChildren<Text>().DOFade(0, 0f);
        
        var anim = mainBoardRT.DOAnchorPosY(0, 0.7f, false).OnComplete(() =>
        {
            if(codeDisplayBackground != null) 
            {
                codeDisplayBackground.GetComponent<Image>().DOFade(0.7f, 0.1f);
                codeDisplayBackground.GetComponentInChildren<Text>().DOFade(1, 0.1f);
            }

            if(codeDisplayRT == null)
            {
                turnIndicator.displayTurn(_initialGameState.currentGameState);
            } else if(codeDisplayRT.anchoredPosition.y != 0) 
            {
                turnIndicator.displayTurn(_initialGameState.currentGameState);
            }

            codeTabScript.displayRoomId();

            print("run main board animation completion handler called");
        });

        codeTabScript.displayRoomId();

        anim.SetDelay(0.4f);
        anim.Play();
    }
}
