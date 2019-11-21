using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

public class MainBoardNetworkingClient : SocketIOComponent
{

    public WordsSelectedAsObject wordsSelectedAsObject; 

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
        On("GameDictionary", (e) => 
        {
            var initialGameDictionary = e.data;
            print(e.data);
        });
    }

    public void sendDictionary(DictionaryAsObject initialGameState)
    {
        var gameStateAsJSONObject = new JSONObject(JsonUtility.ToJson(initialGameState));
        sendWordSelected();

        Emit("GameDictionary", gameStateAsJSONObject);
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
