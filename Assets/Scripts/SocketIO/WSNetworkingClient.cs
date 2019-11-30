using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public abstract class WSNetworkingClient : SocketIOComponent
{
    protected CodeHandlerAbstract codeHandler; 
    public GameState initialGameState;
    public WordsSelectedAsObject wordsSelectedAsObject = new WordsSelectedAsObject();

    protected Team team = Team.BlueTeam;
    protected bool isHosting;
    protected bool isConnected = false;

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
            GameState initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());
        });

        On("wordsSelected", (wordsSelected) =>
        {
            WordsSelectedAsObject wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());
        });

        On("newGameState", (gameState) =>
        {
            CurrentGameState currentGameState = JsonUtility.FromJson<CurrentGameState>(gameState.data.ToString());
        });
    }

    public virtual void sendGameDictionary()
    {
        var gameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(initialGameState));
        Emit("gameDictionary", gameStateAsJSONObject);
        print("dictionary being sent");
    }

    public virtual void sendWordsSelected(List<string> wordsSelected)
    {
        wordsSelectedAsObject.wordsSelected = wordsSelected;
        var wordsSelectedAsJSONObject = new JSONObject(JsonUtility.ToJson(wordsSelectedAsObject));
        Emit("wordsSelected", wordsSelectedAsJSONObject);
    }
    
    public virtual void sendCurrentGameState(CurrentGameState currentGameState)
    {
        var currentGameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(CurrentGameState.gameInPlay));
        Emit("newGameState", currentGameStateAsJSONObject);
    }

    public virtual void joinGameWithCode(string connectionCode)
    {
        ConnectionCodeAsObject connectionCodeAsObject = new ConnectionCodeAsObject();
        connectionCodeAsObject.roomId = connectionCode;
        var connectionCodeAsJSONObject = new JSONObject(JsonUtility.ToJson(connectionCodeAsObject));
        Emit("isJoining", connectionCodeAsJSONObject);
    }
}
