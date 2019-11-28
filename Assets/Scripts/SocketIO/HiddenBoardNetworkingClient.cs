using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;


public class HiddenBoardNetworkingClient : SocketIOComponent
{
    
    public HiddenBoardViewController hiddenBoardViewController; 
    public CodeProviderHandler codeProviderHandler;

    private Team team = Team.BlueTeam;
    private Tabs tab = Tabs.BlueTab;

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

                if (initialGameState.playerIndex == 1)
                {
                    team = Team.RedTeam;
                    tab = Tabs.BlueTab;
                }

                hiddenBoardViewController.blueWords = initialGameState.blueCards;
                hiddenBoardViewController.redWords = initialGameState.redCards;
                hiddenBoardViewController.neutralWords = initialGameState.neutralCards;
                hiddenBoardViewController.shipwreckCardText.text = initialGameState.shipwreckCard[0];
                hiddenBoardViewController.getTextObjectSize();
                hiddenBoardViewController.startTab(tab);

                codeProviderHandler.mainBoardRunningTutorial = initialGameState.isTutorial;
                
                codeProviderHandler.onJoinedRoom(team);
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
    }

    public void joinGameWithCode(string connectionCode)
    {
        ConnectionCodeAsObject connectionCodeAsObject = new ConnectionCodeAsObject();
        connectionCodeAsObject.roomId = connectionCode;
        var connectionCodeAsJSONObject = new JSONObject(JsonUtility.ToJson(connectionCodeAsObject));
        Emit("isJoining", connectionCodeAsJSONObject);
    }
}
