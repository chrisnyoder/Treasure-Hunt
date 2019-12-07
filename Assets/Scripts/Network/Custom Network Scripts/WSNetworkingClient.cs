using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public abstract class WSNetworkingClient : SocketIOComponent
{
    protected CodeHandlerAbstract codeHandler; 
    public GameState initialGameState;
    public WordsSelectedAsObject wordsSelectedAsObject = new WordsSelectedAsObject();
    public CurrentGameStateAsObject currentGameStateAsObject;

    public Team team = Team.BlueTeam;
    protected bool isHosting;
    protected bool isConnected = false;
    protected bool wasDisconnected = false;

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

    public virtual void setupEvents()
    {
        On("open", (e) =>
        {
        });

        On("connect", (e) =>
        {
            isConnected = true;
        });

        On("error", (e) =>
        {
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
            isConnected = false;
            wasDisconnected = true;
            print("disconnect");
            print(e.data.ToString());
        });

        On("reconnecting", (e) =>
        {
            print("reconnecting");
            print(e.data.ToString());
        });

        On("gameDictionary", (dictionary) =>
        {
            initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());
        });

        On("wordsSelected", (wordsSelected) =>
        {
            wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());
        });

        On("newGameState", (gameState) =>
        {
            currentGameStateAsObject = JsonUtility.FromJson<CurrentGameStateAsObject>(gameState.data.ToString());
        });
    }

    public virtual void sendGameDictionary()
    {
        var gameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(initialGameState));

        if(isConnected)
            Emit("gameDictionary", gameStateAsJSONObject);
    }

    public virtual void sendWordsSelected(List<string> wordsSelected)
    {
        wordsSelectedAsObject.wordsSelected = wordsSelected;
        var wordsSelectedAsJSONObject = new JSONObject(JsonUtility.ToJson(wordsSelectedAsObject));

        if(isConnected)
            Emit("wordsSelected", wordsSelectedAsJSONObject);
    }
    
    public virtual void sendCurrentGameState(CurrentGameState currentGameState)
    {
        CurrentGameStateAsObject currentGameStateAsObject = new CurrentGameStateAsObject(currentGameState);
        print("current game state that is about to be sent: " + currentGameStateAsObject.currentGameState);
        var currentGameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(currentGameStateAsObject));

        if(isConnected)
            Emit("newGameState", currentGameStateAsJSONObject);
    }

    public virtual void reconnectToRoomId(string roomId, string role)
    {
        ConnectionCodeAsObject connectionCodeAsObject = new ConnectionCodeAsObject();
        connectionCodeAsObject.roomId = roomId;
        connectionCodeAsObject.role = role;

        print("room id to be sent: " + roomId);
        if(isConnected)
        {
            print("sending that room id");
            Emit("reconnecting", new JSONObject(JsonUtility.ToJson(connectionCodeAsObject)));
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && isConnected)
        {
            Close();
            wasDisconnected = true;
            isConnected = false;
        }
    }

    public void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus && !isConnected)
        {
            Connect();
        }
    }
}
