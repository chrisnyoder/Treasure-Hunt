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

        print("unique device identifer is :" + SystemInfo.deviceUniqueIdentifier);
        var wordProduct =  new WordPackProduct("initialWordList");
        var wordProductList = new List<WordPackProduct>(){wordProduct};

        initialGameState = new GameState(25, wordProductList);
        hiddenBoardViewController.receiveGameStateObject(initialGameState);
        network.netWorkInitialGameState(initialGameState, false);
    }
}
