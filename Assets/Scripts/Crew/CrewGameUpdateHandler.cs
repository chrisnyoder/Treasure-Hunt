using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CrewGameUpdateHandler : MonoBehaviour
{
    JoinGameNetworkingClient joinGameNetworkingClient;
    WordsSelectedAsObject wordsSelectedOnBoard;
    GameState crewMemberGameState; 
    CurrentGameState crewMemberCurrentGameState = CurrentGameState.blueTurn; 

    public GameObject EoGCanvas;
    public GameObject restartingCanvas; 
    private Vector2 initialMainboardPos; 
    private Vector2 initialRestartCanvasPos; 

    CardFlipHandler[] cards;
    public BoardLayoutScript boardLayoutScript;

    private void Start() 
    {
        initialMainboardPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        initialRestartCanvasPos = restartingCanvas.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update() 
    {
        if(wordsSelectedOnBoard.allWordsSelected != joinGameNetworkingClient.wordsSelected.allWordsSelected)
        {
            wordsSelectedOnBoard.allWordsSelected = joinGameNetworkingClient.wordsSelected.allWordsSelected;
            print("words selected are different");
            updateWordsSelected();
        }

        if(crewMemberGameState != joinGameNetworkingClient.initialGameState)
        {
            setUpMainBoardForCrewMember();
        }

        if(crewMemberCurrentGameState != joinGameNetworkingClient.currentGameStateAsObject.currentGameState)
        {
            print("game state on update handler: " + crewMemberCurrentGameState);
            print("game state coming in from network client " + joinGameNetworkingClient.currentGameStateAsObject.currentGameState);
            var gameStateFromServer = joinGameNetworkingClient.currentGameStateAsObject.currentGameState;

            if(gameStateFromServer == CurrentGameState.blueTurn || gameStateFromServer == CurrentGameState.redTurn)
            {
                boardLayoutScript.endTurnHandler.gameState.currentGameState = crewMemberCurrentGameState;
                crewMemberCurrentGameState = joinGameNetworkingClient.currentGameStateAsObject.currentGameState;
                boardLayoutScript.endTurnHandler.changeTurns();
            }
        }   

        if(joinGameNetworkingClient.gameInRestartingState)
        {
            var mainBoardRT = gameObject.GetComponent<RectTransform>();
            mainBoardRT.DOAnchorPosY(initialMainboardPos.y, 0.5f, false).Play().SetEase(Ease.Linear);

            var restartingCanvasRt = restartingCanvas.GetComponent<RectTransform>();
            restartingCanvasRt.DOAnchorPosY(0, 0.5f, false).Play().OnComplete(() =>
            {
                restartingCanvas.GetComponent<Image>().DOFade(0.627f, 0.3f).SetDelay(0.2f);
            });

            exitResultsCanvasIfDisplayed();
        } else 
        {
            var restartingCanvasRt = restartingCanvas.GetComponent<RectTransform>();
            restartingCanvas.GetComponent<Image>().DOFade(0.0f, 0.3f).OnComplete(() => 
            {
                restartingCanvasRt.DOAnchorPosY(initialRestartCanvasPos.y, 0.5f, false).Play();
            });
        }
    }

    private void updateWordsSelected()
    {
        foreach (CardFlipHandler card in cards)
        {
            if (wordsSelectedOnBoard.allWordsSelected.Contains(card.cardText) && !card.cardAlreadyFlipped)
            {
                card.FlipCard();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnJoiningScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnJoiningScene;
    }

    private void OnJoiningScene(Scene scene, LoadSceneMode mode)
    {
        joinGameNetworkingClient = GameObject.Find("NetworkingClient").GetComponent<JoinGameNetworkingClient>();
        setUpMainBoardForCrewMember();
        exitResultsCanvasIfDisplayed();
        print("on joining scene callback being called");
    }

    private void setUpMainBoardForCrewMember()
    {
        if (joinGameNetworkingClient.initialGameState.hiddenBoardList.Count > 0)
        {
            wordsSelectedOnBoard = new WordsSelectedAsObject();
            wordsSelectedOnBoard.allWordsSelected = joinGameNetworkingClient.initialGameState.wordsAlreadySelected;

            boardLayoutScript = GameObject.Find("MainBoardCanvas").GetComponent<BoardLayoutScript>();
            crewMemberGameState = joinGameNetworkingClient.initialGameState;
            crewMemberCurrentGameState = crewMemberGameState.currentGameState;
            boardLayoutScript.receiveGameStateObject(crewMemberGameState);
            
            boardLayoutScript.runMainBoardAnimation();

            cards = gameObject.GetComponentsInChildren<CardFlipHandler>();

            if(cards.Length > 0) 
            {
                foreach (CardFlipHandler card in cards)
                {
                    card.cardAlreadyFlipped = false;
                }
            }
        }

        exitResultsCanvasIfDisplayed();
    }

    private void exitResultsCanvasIfDisplayed()
    {
        if (EoGCanvas.GetComponent<RectTransform>().anchoredPosition.y == 0)
        {
            EoGCanvas.GetComponent<RectTransform>().DOAnchorPosY(1500, 0.7f).Play();
            EoGCanvas.GetComponent<Image>().DOFade(0, 0.1f).Play();
        }
    }
}
