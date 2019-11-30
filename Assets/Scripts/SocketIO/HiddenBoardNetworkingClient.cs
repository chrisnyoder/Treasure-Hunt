using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;


public class HiddenBoardNetworkingClient : WSNetworkingClient
{
    public HiddenBoardViewController hiddenBoardViewController; 
    public CodeProviderHandler codeProviderHandler;

    protected Tabs tab = Tabs.BlueTab;

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
            print("game dictionary callback received: " + dictionary.data.ToString());

            initialGameState = JsonUtility.FromJson<GameState>(dictionary.data.ToString());

            if(initialGameState.hiddenBoardList.Count > 0 ) 
            {
                if (initialGameState.playerIndex == 1)
                {
                    team = Team.RedTeam;
                    tab = Tabs.RedTab;
                }

                var tmpBlueWords = new List<string>();
                var tmpRedWords = new List<string>();
                var tmpNeutralWords = new List<string>();
                var tmpShipwreck = new List<string>();

                foreach(var card in initialGameState.hiddenBoardList)
                {
                    if(card.cardType == CardType.blueCard) { tmpBlueWords.Add(card.labelText); }
                    if(card.cardType == CardType.redCard) { tmpRedWords.Add(card.labelText); }
                    if(card.cardType == CardType.neutralCard) { tmpNeutralWords.Add(card.labelText); }
                    if(card.cardType == CardType.shipwreckCard) { tmpShipwreck.Add(card.labelText); }
                }

                hiddenBoardViewController.blueWords = tmpBlueWords;
                hiddenBoardViewController.redWords = tmpRedWords;
                hiddenBoardViewController.neutralWords = tmpNeutralWords;
                hiddenBoardViewController.shipwreckCardText.text = tmpShipwreck[0];
                hiddenBoardViewController.getTextObjectSize();
                hiddenBoardViewController.startTab(tab);

                codeProviderHandler.mainBoardRunningTutorial = initialGameState.isTutorial;
                
                codeProviderHandler.onJoinedRoom(team);
            }
        });

        On("wordsSelected", (wordsSelected) => {
            
            if(wordsSelectedAsObject.wordsSelected.Count >0 )
                hiddenBoardViewController.wordSelected(wordsSelectedAsObject.wordsSelected); 
        });

        On("newGameState", (gameState) => 
        {   
            CurrentGameState currentGameState = JsonUtility.FromJson<CurrentGameState>(gameState.data.ToString());

            if(currentGameState != CurrentGameState.gameInPlay)
                hiddenBoardViewController.gameStateChanged(currentGameState);
            
        });
    }
}
