using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

public class MainBoardNetworkingClient : SocketIOComponent
{

    [HideInInspector]
    public WordsSelectedAsObject wordsSelectedAsObject;
    [HideInInspector]
    public ConnectionCodeAsObject connectionCodeAsObject;
    public CodeDisplayHandler codeDisplayHandler;
    [HideInInspector]
    public DictionaryAsObject initialGameState; 
    
    public bool dictionarySent = false;
    private bool _connectionMade = false;


    // Start is called before the first frame update
    public override void Start()
    {
        initialGameState = null;

        base.Start();
        setupEvents();

        if(initialGameState == null)
        {
            print("is null");
        } else 
        {
            print("not null");
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 
    
        if(_connectionMade == true && initialGameState != null)
        {
            if(!dictionarySent)
            {
                foreach(var card in initialGameState.blueCards){ print(card);} ;
                print("dictionary sent is false");
                sendDictionary();
                dictionarySent = true;
            }
        }
    }

    private void setupEvents()
    {
        On("open", (e) => {
        });

        On("connect", (e) => {
            _connectionMade = true;
            Emit("isHosting");
        });

        On("error", (e) => {
            print("error");
            print(e.data.ToString());
        });

        On("reconnect", (e) =>
        {
            print("reconnect");
            print(e.data.ToString());
        });

        On("disconnect", (e) =>
        {
            print("disconnect");
            print(e.data.ToString());
        });

        On("reconnecting", (e) =>
        {
            print("reconnecting");
            print(e.data.ToString());
        });
    
        On("roomId", (room) => 
        {
            connectionCodeAsObject = JsonUtility.FromJson<ConnectionCodeAsObject>(room.data.ToString());
            codeDisplayHandler.displayConnectionCode(connectionCodeAsObject.roomId);
        });

        On("numberOfPlayersInRoomChanged", (players) => 
        {
            print("number of players in room has changed, again");
            PlayersAsObject playersAsObject = JsonUtility.FromJson<PlayersAsObject>(players.data.ToString());
            var playerList = playersAsObject.playersInRoom.ToList();

            foreach(var player in playerList){
                print(player);
            }

            codeDisplayHandler.displayPlayersInGame(playerList);
        });

        On("register", (e) => {print("register callback received"); } );
    }

    // public void sendDictionary(DictionaryAsObject initialGameState)
    // {
    //     var gameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(initialGameState));
    //     sendWordSelected();

    //     Emit("gameDictionary", gameStateAsJSONObject);
    // }

    public void sendDictionary()
    {
        print("dictionary being sent");
        var gameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(initialGameState));
        sendWordSelected();

        Emit("gameDictionary", gameStateAsJSONObject);
    }

    public void sendWordSelected()
    {
        var wordsSelectedAsJSONObject = new JSONObject(JsonUtility.ToJson(wordsSelectedAsObject));
        Emit("wordsSelected", wordsSelectedAsJSONObject);
    }

    public void sendNewGameState(CurrentGameState currentGameState)
    {
        var currentGameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(currentGameState));
        Emit("newGameState", currentGameStateAsJSONObject);
    }
}
