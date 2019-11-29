using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;


public class CrewNetworkingClient : SocketIOComponent
{
    
    public CrewBoardCreationScript gameCreation; 
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

            }
        });
    }

    public void joinGameWithCode(string connectionCode)
    {
        ConnectionCodeAsObject connectionCodeAsObject = new ConnectionCodeAsObject();
        connectionCodeAsObject.roomId = connectionCode;
        var connectionCodeAsJSONObject = new JSONObject(JsonUtility.ToJson(connectionCodeAsObject));
        Emit("isJoiningMainBoard", connectionCodeAsJSONObject);
    }
}
