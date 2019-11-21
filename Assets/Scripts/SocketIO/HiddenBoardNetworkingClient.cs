using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SocketIO;


public class HiddenBoardNetworkingClient : SocketIOComponent
{
    
    HiddenBoardViewController hiddenBoardViewController; 

    public override void Start()
    {
        base.Start();
        setupEvents();

        hiddenBoardViewController = GameObject.Find("LayoutObject").GetComponent<HiddenBoardViewController>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void setupEvents()
    {
        On("open", (E) => 
        {
            print("connection made to the server");
        });

        On("GameDictionary", (e) =>
        {
            DictionaryAsObject initialGameState = JsonUtility.FromJson<DictionaryAsObject>(e.data.ToString());

            if(initialGameState.blueCards.Count > 0 ) 
            {
                hiddenBoardViewController.blueWords = initialGameState.blueCards;
                hiddenBoardViewController.redWords = initialGameState.redCards;
                hiddenBoardViewController.neutralWords = initialGameState.neutralCards;
                hiddenBoardViewController.shipwreckCardText.text = initialGameState.shipwreckCard[0];
                hiddenBoardViewController.getTextObjectSize();
            }
        });

        On("wordsSelected", (e) => {
            WordsSelectedAsObject wordsSelectedAsObject = JsonUtility.FromJson<WordsSelectedAsObject>(e.data.ToString());

            if(wordsSelectedAsObject.listOfWordsSelected.Count >0 )
                hiddenBoardViewController.wordSelected(wordsSelectedAsObject.listOfWordsSelected); 
        });

        On("newGameState", (e) => 
        {   
            CurrentGameState currentGameState = JsonUtility.FromJson<CurrentGameState>(e.data.ToString());

            if(currentGameState != CurrentGameState.gameInPlay)
                hiddenBoardViewController.gameStateChanged(currentGameState);
            
        });
    }
}
