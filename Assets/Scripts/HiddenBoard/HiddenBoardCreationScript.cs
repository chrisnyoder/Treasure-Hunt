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

    void Start()
    {
        hiddenBoardViewController.initializeHiddenBoard();
    }
}
