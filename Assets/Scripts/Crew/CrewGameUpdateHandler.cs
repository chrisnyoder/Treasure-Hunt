﻿using System.Collections;
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
    CurrentGameState crewMemberCurrentGameState = CurrentGameState.none; 

    public GameObject EoGCanvas;
    public GameObject restartingCanvas; 
    private Vector2 initialMainboardPos; 
    private Vector2 initialRestartCanvasPos; 

    List<CardFlipHandler> cards;
    public BoardLayoutScript boardLayoutScript;

    private void Awake() {
        Debug.Log("awake function being called on gameobject: " + gameObject.name + " " + gameObject.GetInstanceID());
    }

    private void Start() 
    {
        initialMainboardPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        initialRestartCanvasPos = restartingCanvas.GetComponent<RectTransform>().anchoredPosition;
        print("name of game object");
        print(gameObject.name);
    }

    private void Update() 
    {   
        if(wordsSelectedOnBoard.allWordsSelected != joinGameNetworkingClient.wordsSelected.allWordsSelected)
        {
            print("words seleted are different, updating");
            wordsSelectedOnBoard.allWordsSelected = joinGameNetworkingClient.wordsSelected.allWordsSelected;
            updateWordsSelected();
        }

        if(crewMemberCurrentGameState != joinGameNetworkingClient.networkedGameState.currentGameState)
        {
            var currentGameStateFromServer = joinGameNetworkingClient.networkedGameState.currentGameState;

            switch(currentGameStateFromServer)
            {
                case CurrentGameState.blueTurn:
                    boardLayoutScript.endTurnHandler.changeTurnTo(currentGameStateFromServer);
                    boardLayoutScript.endTurnHandler.gameState.currentGameState = currentGameStateFromServer;
                    break;
                case CurrentGameState.redTurn:
                    boardLayoutScript.endTurnHandler.changeTurnTo(currentGameStateFromServer);
                    boardLayoutScript.endTurnHandler.gameState.currentGameState = currentGameStateFromServer;
                    break;
                case CurrentGameState.blueWins:
                    break;
                case CurrentGameState.redWins:
                    break;
                case CurrentGameState.loses:
                    break;
                case CurrentGameState.restarting:
                    moveMainBoardCanvas();
                    displayRestartingCanvas();
                    exitResultsCavnas();
                    break;
                case CurrentGameState.restarted:
                    moveRestartingCanvas();
                    break;
            }

            if(crewMemberGameState != joinGameNetworkingClient.networkedGameState)
            {
                if(crewMemberGameState != null)
                {   
                    print("game state is different");
                    setUpMainBoardForCrewMember();
                }
            }

            crewMemberCurrentGameState = currentGameStateFromServer;
        }   
    }

    private void updateWordsSelected()
    {
        foreach (CardFlipHandler card in cards)
        {
            if (wordsSelectedOnBoard.allWordsSelected.Contains(card.cardText) && !card.cardAlreadyFlipped)
            {
                card.flipCard();
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
        joinGameNetworkingClient.codeDisplay = GameObject.Find("Game_Id").GetComponent<CodeTabScript>();
        setUpMainBoardForCrewMember();
        exitResultsCavnas();
        print("on joining scene callback being called");
    }

    private void setUpMainBoardForCrewMember()
    {
        if (joinGameNetworkingClient.networkedGameState.hiddenBoardList.Count > 0)
        {
            wordsSelectedOnBoard = new WordsSelectedAsObject();
            wordsSelectedOnBoard.allWordsSelected = joinGameNetworkingClient.networkedGameState.wordsAlreadySelected;

            if (cards != null)
            {
                foreach(CardFlipHandler card in cards) 
                {
                    Destroy(card.gameObject);
                }
            }

            crewMemberGameState = joinGameNetworkingClient.networkedGameState;
            crewMemberCurrentGameState = crewMemberGameState.currentGameState;
            boardLayoutScript.receiveGameStateObject(crewMemberGameState);
            boardLayoutScript.runMainBoardAnimation();            

            cards = new List<CardFlipHandler>(gameObject.GetComponentsInChildren<CardFlipHandler>());

            cards.Reverse();
            cards = cards.GetRange(0, 25);

            print("the number of cards is: " + cards.Count);

            var otherCards = cards[cards.Count - 1];

            if (cards.Count > 0)
            {
                foreach (CardFlipHandler card in cards)
                {
                    card.cardAlreadyFlipped = false;
                }
            }
        }
        exitResultsCavnas();    
    }

    private void displayRestartingCanvas()
    {
        var restartingCanvasRt = restartingCanvas.GetComponent<RectTransform>();

        restartingCanvasRt.DOAnchorPosY(0, 0.5f, false).Play().OnComplete(() =>
        {
            restartingCanvas.GetComponent<Image>().DOFade(0.627f, 0.3f).SetDelay(0.2f);
        });
    }

    private void moveRestartingCanvas()
    {
        var restartingCanvasRt = restartingCanvas.GetComponent<RectTransform>();

        restartingCanvas.GetComponent<Image>().DOFade(0.0f, 0.3f).OnComplete(() =>
        {
            restartingCanvasRt.DOAnchorPosY(initialRestartCanvasPos.y, 0.5f, false).Play();
        });
    }

    private void moveMainBoardCanvas()
    {
        var mainBoardRT = gameObject.GetComponent<RectTransform>();
        mainBoardRT.DOAnchorPosY(initialMainboardPos.y, 0.5f, false).Play().SetEase(Ease.Linear);
    }

    private void exitResultsCavnas()
    {
        EoGCanvas.GetComponent<RectTransform>().DOAnchorPosY(1500, 0.7f).Play();
        EoGCanvas.GetComponent<Image>().DOFade(0, 0.1f).Play();
    }
}
