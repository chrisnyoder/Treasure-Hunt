using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;
using UnityEngine.SceneManagement;

public class JoinGameNetworkingClient : WSNetworkingClient
{    
    public CodeProviderHandler codeProviderHandler;
    public CodeTabScript codeDisplay;
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
        print("joining with code");
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
            networkedGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());
            mainBoardRunningTutorial = networkedGameState.isTutorial;
            wordsSelected.allWordsSelected = networkedGameState.wordsAlreadySelected; 

            if (networkedGameState.playerIndex == 1)
            {
                team = Team.RedTeam;
                tab = Tabs.RedTab;
            }

            if(codeProviderHandler != null)
            {
                print("code provider handler is not null");
                codeProviderHandler.onJoinedRoom(role);
            } else {
                Debug.Log("code provider handler is null");
            }
        });

        On("wordsSelected", (wordsSelected) => {
            base.wordsSelected = JsonUtility.FromJson<WordsSelectedAsObject>(wordsSelected.data.ToString());
        });

        On("newGameState", (gameState) => {
            CurrentGameStateAsObject currentGameStateAsObject = new CurrentGameStateAsObject(CurrentGameState.none);
            currentGameStateAsObject = JsonUtility.FromJson<CurrentGameStateAsObject>(gameState.data.ToString());
            networkedGameState.currentGameState = currentGameStateAsObject.currentGameState;
            print("new game state received in join game client: " + currentGameStateAsObject.currentGameState);
        });

        On("disconnect", (e) => {
            wasDisconnected = true;
            Connect();
        });

        On("timer", (timerData) =>
        {
            timerObject = JsonUtility.FromJson<TimerAsObject>(timerData.data.ToString());
        });

        On("unpausing", (data) =>
        {
            print("game unpausing");
            gamePaused = false;
        });

        On("pausing", (data) =>
        {
            print("game pausing");
            gamePaused = true; 
        });
    }
}
