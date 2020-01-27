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
    private CurrentGameState _hiddenBoardCurrentGameState = CurrentGameState.none;
    private List<string> wordsSelected = new List<string>(){};  
    private JoinGameNetworkingClient joinGameNetworkingClient;

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
            if (hiddenBoardGameDictionary != joinGameNetworkingClient.networkedGameState.hiddenBoardList)
            {
                print("hidden board is different");
                hiddenBoardGameDictionary = joinGameNetworkingClient.networkedGameState.hiddenBoardList;
                hiddenBoardViewController.wordSelected(wordsSelected);
                setUpHiddenBoard();
            }

            if (wordsSelected != joinGameNetworkingClient.wordsSelected.allWordsSelected)
            {
                print("words are different");
                wordsSelected = joinGameNetworkingClient.wordsSelected.allWordsSelected;
                hiddenBoardViewController.wordSelected(wordsSelected);
            }

            if (_hiddenBoardCurrentGameState != joinGameNetworkingClient.networkedGameState.currentGameState)
            {
                print("current game state: " + _hiddenBoardCurrentGameState);
                print("new game state: " + joinGameNetworkingClient.networkedGameState.currentGameState);
                _hiddenBoardCurrentGameState = joinGameNetworkingClient.networkedGameState.currentGameState;
                hiddenBoardViewController.gameStateChanged(_hiddenBoardCurrentGameState);
            }
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
        _hiddenBoardCurrentGameState = joinGameNetworkingClient.networkedGameState.currentGameState;
        hiddenBoardViewController.gameStateChanged(_hiddenBoardCurrentGameState);
        hiddenBoardGameDictionary = joinGameNetworkingClient.networkedGameState.hiddenBoardList;
        hiddenBoardViewController.wordsSelected = wordsSelected;

        setUpHiddenBoard();
        strikethroughSelectedWords();
        runTutorialIfNecessary();
    }

    private void setUpHiddenBoard()
    {
        if (joinGameNetworkingClient.networkedGameState.hiddenBoardList.Count > 0)
        {
            var tmpBlueWords = new List<string>();
            var tmpRedWords = new List<string>();
            var tmpNeutralWords = new List<string>();
            var tmpShipwreck = new List<string>();

            foreach (var card in joinGameNetworkingClient.networkedGameState.hiddenBoardList)
            {
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
}
