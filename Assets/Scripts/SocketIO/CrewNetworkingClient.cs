using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;
using DG.Tweening;

public class CrewNetworkingClient : WSNetworkingClient
{
    public BoardLayoutScript boardLayoutScript;
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

    public override void setupEvents()
    {
        base.setupEvents();

        On("gameDictionary", (dictionary) =>
        {
            GameState initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());

            if(initialGameState.hiddenBoardList.Count > 0 ) 
            {
                boardLayoutScript.receiveGameStateObject(initialGameState);
            }

            codeProviderHandler.onJoinedRoom(Team.BlueTeam);
        });

        On("wordsSelected", (wordsSelected) => { });

        On("newGameState", (newGameState) => { });
    }
}
