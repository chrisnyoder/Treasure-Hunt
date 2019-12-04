using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CrewGameUpdateHandler : MonoBehaviour
{
    JoinGameNetworkingClient joinGameNetworkingClient;
    WordsSelectedAsObject wordsSelectedAsObject;
    GameState crewMemberGameState; 
    public GameObject EoGCanvas;

    CardFlipHandler[] cards;
    public BoardLayoutScript boardLayoutScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update() 
    {
        if(joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected != wordsSelectedAsObject.wordsSelected)
        {
            print("words selected are different");
            updateWordsSelected();
        }

        if(crewMemberGameState != joinGameNetworkingClient.initialGameState)
        {
            setUpMainBoardForCrewMember();
        }
    }

    private void updateWordsSelected()
    {
        wordsSelectedAsObject.wordsSelected = joinGameNetworkingClient.wordsSelectedAsObject.wordsSelected;
        foreach (CardFlipHandler card in cards)
        {
            print("card is flipped: " + card.cardIsFlipped);
            if (card.cardText == wordsSelectedAsObject.lastWordSelected && !card.cardIsFlipped)
            {
                print("flipping card");
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
    }

    private void setUpMainBoardForCrewMember()
    {
        if (joinGameNetworkingClient.initialGameState.hiddenBoardList.Count > 0)
        {
            wordsSelectedAsObject = new WordsSelectedAsObject();
            wordsSelectedAsObject.wordsSelected = joinGameNetworkingClient.initialGameState.wordsSelected;

            boardLayoutScript = GameObject.Find("MainBoardCanvas").GetComponent<BoardLayoutScript>();
            crewMemberGameState = joinGameNetworkingClient.initialGameState;
            boardLayoutScript.receiveGameStateObject(crewMemberGameState);
            
            var rt = gameObject.GetComponent<RectTransform>();
            rt.DOAnchorPosY(0, 0.5f, false).Play().SetEase(Ease.Linear);

            cards = gameObject.GetComponentsInChildren<CardFlipHandler>();

            if(cards.Length > 0) 
            {
                foreach (CardFlipHandler card in cards)
                {
                    card.cardIsFlipped = false;
                }
            }
        }

        exitResultsCanvasIfDisplayed();
    }

    private void exitResultsCanvasIfDisplayed()
    {
        if (EoGCanvas.GetComponent<RectTransform>().anchoredPosition.y == 0)
        {
            EoGCanvas.GetComponent<Animator>().Play("ResultsAnimationReverse");
        }
    }
}
