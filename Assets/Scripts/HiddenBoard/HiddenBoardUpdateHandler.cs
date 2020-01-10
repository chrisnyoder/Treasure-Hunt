using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class HiddenBoardUpdateHandler : MonoBehaviour
{
    public HiddenBoardViewController hiddenBoardViewController;
    
    public GameObject restartingCanvas;
    public EoGScript eoGScript;
    private Vector2 initialRestartCanvasPos;

    private List<CardObject> hiddenBoardGameDictionary = new List<CardObject>(); 
    private CurrentGameState currentGameState = CurrentGameState.gameInPlay;
    private List<string> wordsSelected = new List<string>(){};  
    private JoinGameNetworkingClient joinGameNetworkingClient;

    private void Awake() {
        currentGameState = CurrentGameState.gameInPlay;
    }

    private void Start() {
        initialRestartCanvasPos = restartingCanvas.GetComponent<RectTransform>().anchoredPosition;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnJoiningScene;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnJoiningScene;
    }

    private void Update() 
    {
        if(joinGameNetworkingClient != null) 
        {
            if (hiddenBoardGameDictionary != joinGameNetworkingClient.initialGameState.hiddenBoardList)
            {
                print("hidden board is different");
                hiddenBoardGameDictionary = joinGameNetworkingClient.initialGameState.hiddenBoardList;
                hiddenBoardViewController.wordSelected(wordsSelected);
                setUpHiddenBoard();
            }

            if (wordsSelected != joinGameNetworkingClient.wordsSelected.allWordsSelected)
            {
                print("words are different");
                wordsSelected = joinGameNetworkingClient.wordsSelected.allWordsSelected;
                hiddenBoardViewController.wordSelected(wordsSelected);
            }

            if (currentGameState != joinGameNetworkingClient.currentGameStateAsObject.currentGameState)
            {
                print("game state is different");
                currentGameState = joinGameNetworkingClient.currentGameStateAsObject.currentGameState;
                hiddenBoardViewController.gameStateChanged(currentGameState);
            }
        }

        if (joinGameNetworkingClient.gameInRestartingState)
        {
            var restartingCanvasRt = restartingCanvas.GetComponent<RectTransform>();
            restartingCanvasRt.DOAnchorPosY(0, 0.5f, false).Play().OnComplete(() =>
            {
                restartingCanvas.GetComponent<Image>().DOFade(0.627f, 0.3f).SetDelay(0.2f);
            });

            exitResultsCanvasIfDisplayed();
        }
        else
        {
            var restartingCanvasRt = restartingCanvas.GetComponent<RectTransform>();
            restartingCanvas.GetComponent<Image>().DOFade(0.0f, 0.3f).OnComplete(() =>
            {
                restartingCanvasRt.DOAnchorPosY(initialRestartCanvasPos.y, 0.5f, false).Play();
            });
        }
    }

    private void OnJoiningScene(Scene scene, LoadSceneMode mode)
    {
        print("on scene loading called");
        if(joinGameNetworkingClient == null)
        {
            print("join game networkign client being asigned");
            joinGameNetworkingClient = GameObject.Find("NetworkingClient").GetComponent<JoinGameNetworkingClient>();
        }
        
        hiddenBoardViewController.initializeHiddenBoard(joinGameNetworkingClient.tab);
        hiddenBoardGameDictionary = joinGameNetworkingClient.initialGameState.hiddenBoardList;
        hiddenBoardViewController.wordsSelected = wordsSelected;

        setUpHiddenBoard();
        strikethroughSelectedWords();
        runTutorialIfNecessary();
    }

    private void setUpHiddenBoard()
    {
        if (joinGameNetworkingClient.initialGameState.hiddenBoardList.Count > 0)
        {
            print("hidden board list greater than 1");

            var tmpBlueWords = new List<string>();
            var tmpRedWords = new List<string>();
            var tmpNeutralWords = new List<string>();
            var tmpShipwreck = new List<string>();

            foreach (var card in joinGameNetworkingClient.initialGameState.hiddenBoardList)
            {
                print("label text is: " + card.labelText);

                if (card.cardType == CardType.blueCard) { tmpBlueWords.Add(card.labelText); }
                if (card.cardType == CardType.redCard) { tmpRedWords.Add(card.labelText); }
                if (card.cardType == CardType.neutralCard) { tmpNeutralWords.Add(card.labelText); }
                if (card.cardType == CardType.shipwreckCard) { tmpShipwreck.Add(card.labelText); }
            }

            hiddenBoardViewController.blueWords = tmpBlueWords;
            hiddenBoardViewController.redWords = tmpRedWords;
            hiddenBoardViewController.neutralWords = tmpNeutralWords;
            hiddenBoardViewController.shipwreckCardText.text = tmpShipwreck[0];

            hiddenBoardViewController.getTextObjectSize();
        }
    }

    private void strikethroughSelectedWords()
    {
        foreach (var word in joinGameNetworkingClient.wordsSelected.allWordsSelected)
        {
            print(word);
        }

        if (joinGameNetworkingClient.wordsSelected.allWordsSelected.Count > 0)
            hiddenBoardViewController.wordSelected(joinGameNetworkingClient.wordsSelected.allWordsSelected);
    }

    private void runTutorialIfNecessary()
    {
        if (joinGameNetworkingClient.mainBoardRunningTutorial)
        {
            TutorialHiddenBoardScript tutorialHiddenBoardScript = GameObject.Find("TutorialCanvas").GetComponent<TutorialHiddenBoardScript>();
            tutorialHiddenBoardScript.beginTutorial(joinGameNetworkingClient.team);
        }
    }

    private void exitResultsCanvasIfDisplayed()
    {
        var rt = eoGScript.GetComponent<RectTransform>();
        if (rt.anchoredPosition.y == 0)
        {
            var EoGCanvasObject = eoGScript.gameObject;
            EoGCanvasObject.GetComponent<Image>().DOFade(0, 0.1f).Play();
            if (Screen.width < Screen.height)
            {
                EoGCanvasObject.GetComponent<RectTransform>().DOAnchorPosY(3000, 1f).Play();
            }
            else
            {
                EoGCanvasObject.GetComponent<RectTransform>().DOAnchorPosY(1500, 0.7f).Play();
            }
        }
    }
}
