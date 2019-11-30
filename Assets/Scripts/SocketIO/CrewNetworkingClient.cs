using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;
using DG.Tweening;

public class CrewNetworkingClient : WSNetworkingClient
{
    public BoardLayoutScript boardLayoutScript;
    public CodeProviderHandler codeProviderHandler;
    public GameObject collectionView;
    private CardFlipHandler[] cards;

    public override void Start()
    {
        base.Start();
        setupEvents();
        cards = collectionView.GetComponentsInChildren<CardFlipHandler>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void setupEvents()
    {
        base.setupEvents();

        On("gameDictionary", (dictionary) =>
        {
            GameState initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());

            if(initialGameState.hiddenBoardList.Count > 0 ) 
            {
                boardLayoutScript.receiveGameStateObject(initialGameState);
            }

            codeProviderHandler.onJoinedRoom(Team.BlueTeam);
        });

        On("wordsSelected", (wordsSelected) => {
            WordsSelectedAsObject wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());

            CardFlipHandler[] cards = collectionView.GetComponentsInChildren<CardFlipHandler>();
            foreach (CardFlipHandler card in cards)
            {
                if (card.cardText == wordsSelectedAsObject.lastWordSelected && !card.cardIsFlipped)
                {
                    card.FlipCard();
                }
            }
         });

        On("newGameState", (newGameState) => { });
    }
}
