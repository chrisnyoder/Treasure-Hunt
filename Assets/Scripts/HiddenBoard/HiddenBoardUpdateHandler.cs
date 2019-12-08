using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiddenBoardUpdateHandler : MonoBehaviour
{
    public HiddenBoardViewController hiddenBoardViewController;
    private List<CardObject> hiddenBoardGameDictionary = new List<CardObject>(); 
    private CurrentGameState currentGameState = CurrentGameState.gameInPlay;
    private List<string> wordsSelected = new List<string>(){};  
    private JoinGameNetworkingClient joinGameNetworkingClient;

    private void Awake() {
        currentGameState = CurrentGameState.gameInPlay;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnJoiningScene;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnJoiningScene;
    }

    private void Update() 
    {
        if(hiddenBoardGameDictionary != joinGameNetworkingClient.initialGameState.hiddenBoardList)
        {
            print("hidden board is different");
            hiddenBoardViewController.wordsSelected = joinGameNetworkingClient.initialGameState.wordsSelected;
            hiddenBoardGameDictionary = joinGameNetworkingClient.initialGameState.hiddenBoardList;
            hiddenBoardViewController.wordSelected(wordsSelected);
            setUpHiddenBoard();
        }

        if(wordsSelected != joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected)
        {
            print("words are different");
            wordsSelected = joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected;
            hiddenBoardViewController.wordSelected(wordsSelected);
        }

        if(currentGameState != joinGameNetworkingClient.currentGameStateAsObject.currentGameState)
        {
            print("game state is different");
            currentGameState = joinGameNetworkingClient.currentGameStateAsObject.currentGameState;
            hiddenBoardViewController.gameStateChanged(currentGameState);
        }
    }

    private void OnJoiningScene(Scene scene, LoadSceneMode mode)
    {
        if(joinGameNetworkingClient == null)
        {
            joinGameNetworkingClient = GameObject.Find("NetworkingClient").GetComponent<JoinGameNetworkingClient>();
        }
        
        hiddenBoardViewController.initializeHiddenBoard(joinGameNetworkingClient.tab);
        hiddenBoardGameDictionary = joinGameNetworkingClient.initialGameState.hiddenBoardList;
        hiddenBoardViewController.wordsSelected = joinGameNetworkingClient.initialGameState.wordsSelected;

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
        foreach (var word in joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected)
        {
            print(word);
        }

        if (joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected.Count > 0)
            hiddenBoardViewController.wordSelected(joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected);
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
