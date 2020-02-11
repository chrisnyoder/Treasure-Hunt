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
    public RectTransform roomCodeRT; 
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

    private List<string> notchedDevices; 

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

        notchedDevices = new List<string>(){"iPhone10,3", "iPhone10,6", "iPhone10,8", "iPhone11,2", "iPhone11,6", "iPhone11,4", "iPhone12,1", "iPhone12,3", "iPhone12,5"};
    }

    private void Start() 
    {
        if(GlobalDefaults.Instance.isTablet)
        {
            print("global defaults detecting tablet");

            var backgroundImage = this.GetComponent<Image>();
            var tabletBackgroundImage = Resources.Load<Sprite>("Images/Backgrounds/iPad_12_MB_Background");
            moveMenu();
            moveEndTurnButton();
            rotateEndTurnButton();

            if(tabletBackgroundImage != null && backgroundImage != null)
            {
                backgroundImage.sprite = tabletBackgroundImage;
            }
        }

        if(notchedDevices.Contains(SystemInfo.deviceModel))
        {
            layoutForNotchedDevices();
        }; 
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
        makeEndTurnButtonAppropriatelyColor();
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
            boardWidth = mainBoardRT.rect.width;
            cardWidth = (boardWidth * 0.9f) / 5;
            cardHeight = (float)(cardWidth*0.6316);
            totalSpacing = mainBoardRT.rect.width - (cardWidth * 5) - 100;

            collectionViewRT.sizeDelta = new Vector2((boardWidth-totalSpacing), (boardWidth-totalSpacing)*0.6316f);
        }
        else
        {
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

        Debug.Log("objects being destroyed");

        if (cardPositions == null)
        {
            cardPositions = new RectTransform[numberOfCards];
        } 
        else
        {
            Debug.Log("number of objects in the rectransfom array: " + cardPositions.Length);
            foreach(RectTransform rt in cardPositions)
            { 
                Destroy(rt.gameObject);
            }
        }

        Debug.Log("creating cards");
    
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

        Debug.Log("objects being destroyed");

        layoutCards();
    }

    void layoutCards()
    {
        if(isTablet)
        {
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

    private void layoutForNotchedDevices()
    {
        menuParentRT.localPosition = new Vector2(menuParentRT.localPosition.x - 90, menuParentRT.localPosition.y);
        roomCodeRT.localPosition = new Vector2(roomCodeRT.localPosition.x - 120, roomCodeRT.localPosition.y);
    }

    void moveMenu()
    {
        menuParentRT.localPosition = new Vector2(menuParentRT.localPosition.x - 30, menuParentRT.localPosition.y+20);
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

    void makeEndTurnButtonAppropriatelyColor()
    {
        print("end turn button being changed to the correct color");
        switch(_initialGameState.currentGameState)
        {
            case CurrentGameState.blueTurn:
                endTurnHandler.timerFill.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_blue");
                break;
            case CurrentGameState.redTurn:
                endTurnHandler.timerFill.sprite = Resources.Load<Sprite>("Images/MainBoard/timerbar_red");
                break;
        }
    }

    public void dismissCodeDisplay()
    {
        if(codeDisplayRT != null) 
        {
            codeDisplayRT.DOAnchorPosY(-2000, 1f, false);
            codeDisplayRT.GetComponent<Image>().DOFade(0, 0.3f);
        }

        startGame();
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

            if(codeDisplayRT == null)
            {
                if(_initialGameState.currentGameState == CurrentGameState.blueTurn || _initialGameState.currentGameState == CurrentGameState.redTurn) 
                {
                    joinGame();
                }
            } else if(codeDisplayRT.anchoredPosition.y != 0) 
            {
                startGame();
            }

            codeTabScript.displayRoomId();
            menuParentRT.gameObject.SetActive(true);

        });

        codeTabScript.displayRoomId();

        anim.SetDelay(0.4f);
        anim.Play();
    }

    public void startGame()
    {
        _initialGameState.currentGameState = CurrentGameState.blueTurn;
        networkingClient.sendCurrentGameState(_initialGameState.currentGameState);
        turnIndicator.displayTurn(_initialGameState.currentGameState);
        turnIndicator.sendInitialGameStateToServer();
    }

    private void joinGame()
    {
        turnIndicator.displayTurn(_initialGameState.currentGameState);
    }
}
