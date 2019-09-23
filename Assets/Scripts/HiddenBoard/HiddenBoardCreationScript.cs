using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBoardCreationScript : MonoBehaviour
{
    GameState initialGameState;
    public HiddenBoardViewController hiddenBoardViewController;
    public Network network;

    void Start()
    {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();

        network.StartClient(dictionaryReceivedCallback);
        hiddenBoardViewController.initializeHiddenBoard();
    }

    void dictionaryReceivedCallback(Dictionary<CardType, List<string>> dict)
    {
        hiddenBoardViewController.receiveMainBoardDictionary(dict);
    }
}
