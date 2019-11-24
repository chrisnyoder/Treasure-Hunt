using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;


public class HiddenBoardNetworkingClient : SocketIOComponent
{
    
    public HiddenBoardViewController hiddenBoardViewController; 
    public CodeProviderHandler codeProviderHandler;

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
        On("open", (e) => 
        {
            print("connection to the server open");
        });

        On("connect", (e) => {
            // connected
        });

        On("gameDictionary", (dictionary) =>
        {
            DictionaryAsObject initialGameState = JsonUtility.FromJson<DictionaryAsObject>(dictionary.data.ToString());

            if(initialGameState.blueCards.Count > 0 ) 
            {
                hiddenBoardViewController.blueWords = initialGameState.blueCards;
                hiddenBoardViewController.redWords = initialGameState.redCards;
                hiddenBoardViewController.neutralWords = initialGameState.neutralCards;
                hiddenBoardViewController.shipwreckCardText.text = initialGameState.shipwreckCard[0];
                hiddenBoardViewController.getTextObjectSize();
            }
        });

        On("wordsSelected", (wordsSelected) => {
            WordsSelectedAsObject wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());

            if(wordsSelectedAsObject.listOfWordsSelected.Count >0 )
                hiddenBoardViewController.wordSelected(wordsSelectedAsObject.listOfWordsSelected); 
        });

        On("newGameState", (gameState) => 
        {   
            CurrentGameState currentGameState = JsonUtility.FromJson<CurrentGameState>(gameState.data.ToString());

            if(currentGameState != CurrentGameState.gameInPlay)
                hiddenBoardViewController.gameStateChanged(currentGameState);
            
        });

        On("joinedRoom", (E) => {
            codeProviderHandler.onJoinedRoom();
        });
    }

    public void joinGameWithCode(string connectionCode)
    {
        ConnectionCodeAsObject connectionCodeAsObject = new ConnectionCodeAsObject();
        connectionCodeAsObject.roomId = connectionCode;
        var connectionCodeAsJSONObject = new JSONObject(JsonUtility.ToJson(connectionCodeAsObject));
        Emit("isJoining", connectionCodeAsJSONObject);
    }
}
