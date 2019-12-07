using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

public class MainBoardNetworkingClient : WSNetworkingClient
{
    public CodeTabScript codeTab;
    public GameObject collectionView; 
    private string roomId; 

    [HideInInspector]
    public ConnectionCodeAsObject connectionCodeAsObject;
    public CodeDisplayHandler codeDisplayHandler;
    
    public bool gameStateSent = false;

    private bool fetchedRoomState = false;

    // Start is called before the first frame update
    public override void Start()
    {
        initialGameState = null;

        base.Start();
        setupEvents();
        Connect();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 
    
        if (isConnected == true && initialGameState != null)
        {
            if (!gameStateSent)
            {
                print("about to send dictionary");
                sendGameDictionary();
                sendCurrentGameState(CurrentGameState.gameInPlay);
                gameStateSent = true;
            }
        }
    }

    public override void setupEvents()
    {
        base.setupEvents();

        On("connect", (e) =>
        {
            isConnected = true;
            print("connect callback received");
            if(!fetchedRoomState)
            {
                print("room not yet generated, generating room");
                isHosting = true;
                Emit("isHosting");
                fetchedRoomState = true;
            } 
            
            if(wasDisconnected)
            {
                print("room id was already fetched, so will try to find it on the server instead");
                reconnectToRoomId(roomId, "isHosting");
                wasDisconnected = false;
            }
        });

        On("connetion", (e) => {
            print("on connection callback reveived");
        });
    
        On("roomId", (room) => 
        {
            connectionCodeAsObject = JsonUtility.FromJson<ConnectionCodeAsObject>(room.data.ToString());
            codeDisplayHandler.displayConnectionCode(connectionCodeAsObject.roomId);
            codeTab.updateConnectionCode(connectionCodeAsObject.roomId);
            roomId = connectionCodeAsObject.roomId;
            fetchedRoomState = true; 
        });

        On("numberOfPlayersInRoomChanged", (players) => 
        {
            PlayersAsObject playersAsObject = JsonUtility.FromJson<PlayersAsObject>(players.data.ToString());
            var playerList = playersAsObject.playersInRoom.ToList();

            codeDisplayHandler.displayPlayersInGame(playerList);
        });
        
        On("wordsSelected", (wordsSelected) => 
        {
            WordsSelectedAsObject wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());

            CardFlipHandler[] cards = collectionView.GetComponentsInChildren<CardFlipHandler>();
            foreach(CardFlipHandler card in cards) {
                if(card.cardText == wordsSelectedAsObject.lastWordSelected && !card.cardIsFlipped) 
                {
                    card.FlipCard();
                }
            }
        });

        On("newGameState", (newGameState) => {} );
    }
}
