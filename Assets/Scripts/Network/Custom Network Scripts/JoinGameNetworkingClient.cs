using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;
using UnityEngine.SceneManagement;

public class JoinGameNetworkingClient : WSNetworkingClient
{    
    public CodeProviderHandler codeProviderHandler;
    public Role role;
    public bool mainBoardRunningTutorial;

    public Tabs tab = Tabs.BlueTab;

    public override void Start()
    {
        base.Start();
        setupEvents();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void joinGameWithCode(string connectionCode)
    {
        ConnectionCodeAsObject connectionCodeAsObject = new ConnectionCodeAsObject();
        connectionCodeAsObject.roomId = connectionCode;
        var connectionCodeAsJSONObject = new JSONObject(JsonUtility.ToJson(connectionCodeAsObject));

        switch(role) {
            case Role.captain:
                Emit("isJoining", connectionCodeAsJSONObject);
                break;
            case Role.crew:
                Emit("isJoiningMainBoard", connectionCodeAsJSONObject);
                break;
        }   
    }

    public override void setupEvents()
    {
        base.setupEvents();

        On("gameDictionary", (dictionary) =>
        {
            initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());
            mainBoardRunningTutorial = initialGameState.isTutorial;
            wordsSelectedAsObject.wordsSelected = initialGameState.wordsSelected; 

            if (initialGameState.playerIndex == 1)
            {
                team = Team.RedTeam;
                tab = Tabs.RedTab;
            }

            codeProviderHandler.onJoinedRoom(role);
        });

        On("wordsSelected", (wordsSelected) => {
            wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());
        });

        On("newGameState", (gameState) => 
        {   
            currentGameStateAsObject = JsonUtility.FromJson<CurrentGameStateAsObject>(gameState.data.ToString());  
            CurrentGameState newGameState = currentGameStateAsObject.currentGameState;

            print("print new game state: " + currentGameStateAsObject.currentGameState);
        });
    }
}
