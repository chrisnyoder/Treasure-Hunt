using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Linq;

public abstract class WSNetworkingClient : SocketIOComponent
{
    protected CodeHandlerAbstract codeHandler; 
    public GameState networkedGameState;
    public WordsSelectedAsObject wordsSelected = new WordsSelectedAsObject();
    public TimerAsObject timerObject = new TimerAsObject();
    public string roomId; 
    public bool gamePaused = false; 

    public List<string> wordsSelectedQueue = new List<string>(){}; 

    public Team team = Team.BlueTeam;
    protected bool isHosting;
    public bool isConnected = false;
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
        if(wordsSelectedQueue.Count > 0 && isConnected)
        {
            sendWordsSelected(wordsSelectedQueue.First());
            wordsSelectedQueue.Remove(wordsSelectedQueue.First());
        }
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
            networkedGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());
        });

        On("wordsSelected", (wordsSelected) =>
        {
            this.wordsSelected = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());
        });

        On("newGameState", (gameState) =>
        {
        });

        On("timer", (timerData) => 
        {
        });
    }

    public virtual void sendGameDictionary()
    {
        var gameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(networkedGameState));

        if(isConnected)
            Emit("gameDictionary", gameStateAsJSONObject);
    }

    public virtual void sendWordsSelected(string wordSelected)
    {
        this.wordsSelected.wordSelected = wordSelected;
        var wordsSelectedAsJSONObject = new JSONObject(JsonUtility.ToJson(this.wordsSelected));

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

    public virtual void pauseGame(Action<JSONObject> callback)
    {
        Emit("pausing", callback);
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && isConnected)
        {
            print("on application paused called");
            Close();
            wasDisconnected = true;
            isConnected = false;
        }
    }

    public void OnApplicationFocus(bool focusStatus)
    {
        print("on application focused called");
        if (focusStatus && !isConnected)
        {
            print("connecting...");
            Connect();
        }
    }
}
