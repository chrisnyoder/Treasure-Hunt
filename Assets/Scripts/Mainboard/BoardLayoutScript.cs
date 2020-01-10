using System.Collections;
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
    public GameObject buttonParentObject;
    public GameObject collectionView;
    public GameObject musicButton; 
    public GameObject exitButton; 

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

    private void Start() 
    {
        musicButton.GetComponent<RectTransform>().sizeDelta = exitButton.GetComponent<RectTransform>().sizeDelta;
        musicButton.GetComponent<RectTransform>().localPosition = new Vector2(exitButton.GetComponent<RectTransform>().localPosition.x, (exitButton.GetComponent<RectTransform>().localPosition.y - (exitButton.GetComponent<RectTransform>().sizeDelta.y/2)) - 75);

        if(GlobalDefaults.Instance.isTablet)
        {
            var backgroundImage = this.GetComponent<Image>();
            var tabletBackgroundImage = Resources.Load<Sprite>("Images/Backgrounds/iPad_12_MB_Background");
            exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -60);
            musicButton.GetComponent<RectTransform>().localPosition = new Vector2(exitButton.GetComponent<RectTransform>().localPosition.x - 80, exitButton.GetComponent<RectTransform>().localPosition.y);

            if(tabletBackgroundImage != null && backgroundImage != null)
            {
                backgroundImage.sprite = tabletBackgroundImage;
            }
        }    
    }

    public void receiveGameStateObject(GameState initialGameState)
    {
        this.initialGameState = initialGameState;

        mainBoardRT = mainBoard.GetComponent<RectTransform>();
        
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
            cardHeight = (float)(cardWidth*0.6316);
            totalSpacing = mainBoardRT.rect.width - (cardWidth * 5);

            collectionViewRT.sizeDelta = new Vector2(boardWidth, boardWidth*0.6316f);
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
        var image = buttonParentObject.GetComponentsInChildren<Image>();
        image[1].rectTransform.sizeDelta = buttonParentRT.sizeDelta;

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

            if(initialGameState.wordsAlreadySelected.Contains(cardText.text))
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

    public void runMainBoardAnimation()
    {
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
            codeTabScript.showTab();
        });

        anim.SetDelay(0.4f);
        anim.Play();
    }
}
