using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

public class MainBoardNetworkingClient : SocketIOComponent
{

    [HideInInspector]
    public WordsSelectedAsObject wordsSelectedAsObject;
    [HideInInspector]
    public ConnectionCodeAsObject connectionCodeAsObject;

    public CodeDisplayHandler codeDisplayHandler;
    
    private bool connectionOpen = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        setupEvents();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 
    }

    private void setupEvents()
    {
        On("open", (e) => {
        });

        On("connect", (e) => {
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
            print("connection code as JSON is: " + room.data.ToString());
            connectionCodeAsObject = JsonUtility.FromJson<ConnectionCodeAsObject>(room.data.ToString());

            print("connmection code: " + connectionCodeAsObject.roomId);

            codeDisplayHandler.displayConnectionCode(connectionCodeAsObject.roomId);
        });

        On("register", (e) => {print("register callback received"); } );
    }

    public void sendDictionary(DictionaryAsObject initialGameState)
    {
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
