using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HiddenBoardCallbackType
{
    DictionaryReceived, 
    GameStateChanged,
    WordSelected, 
    Language 
}

public class HiddenBoardCreationScript : MonoBehaviour
{
    GameState initialGameState;
    public HiddenBoardViewController hiddenBoardViewController;
    public Network network;

    void Start()
    {
        network.ProvideHiddenBoardNetworkCallbacks(sendDictionaryToHiddenBoard, sendWordSelectedToHiddenBoard, sendUpdatedGameStateToHiddenBoard, sendLanguageToHiddenBoard);
        network.setNetworkAsClient();
        hiddenBoardViewController.initializeHiddenBoard();
    }

    void sendDictionaryToHiddenBoard(Dictionary<CardType, List<string>> initialMainBoardDict)
    {
        hiddenBoardViewController.receiveMainBoardDictionary(initialMainBoardDict);
    }

    void sendWordSelectedToHiddenBoard(Dictionary<string, bool> wordsSelected)
    {

    }

    void sendUpdatedGameStateToHiddenBoard(CurrentGameState currentGameState)
    {

    }

    void sendLanguageToHiddenBoard(string language)
    {

    }
}
