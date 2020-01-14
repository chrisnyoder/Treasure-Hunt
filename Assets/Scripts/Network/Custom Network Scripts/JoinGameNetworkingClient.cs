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
    public bool gameInRestartingState = false;

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
        roomId = connectionCode;
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

        On("connect", (e) => {
            isConnected = true;
            print("connect callback received");

            if (wasDisconnected)
            {
                switch (role)
                {
                    case Role.captain:
                        print("sending reconnect function as captain");
                        reconnectToRoomId(roomId, "isNotHosting");
                        break;
                    case Role.crew:
                        reconnectToRoomId(roomId, "isHosting");
                        break;
                }
                wasDisconnected = false;
            }
        });

        On("gameDictionary", (dictionary) => {
            initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());
            mainBoardRunningTutorial = initialGameState.isTutorial;
            wordsSelected.allWordsSelected = initialGameState.wordsAlreadySelected; 

            if (initialGameState.playerIndex == 1)
            {
                team = Team.RedTeam;
                tab = Tabs.RedTab;
            }

            codeProviderHandler.onJoinedRoom(role);
            gameInRestartingState = false;
        });

        On("wordsSelected", (wordsSelected) => {
            print("words selected callback received for joining client");
            base.wordsSelected = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());
        });

        On("newGameState", (gameState) => {   
            currentGameStateAsObject = JsonUtility.FromJson<CurrentGameStateAsObject>(gameState.data.ToString());  
            gameInRestartingState = false;
        });

        On("disconnect", (e) => {
            wasDisconnected = true;
            Connect();
        });

        On("restarting", (e) => {
            print("restart message received on client machines");
            gameInRestartingState = true; 
        });
    }
}
