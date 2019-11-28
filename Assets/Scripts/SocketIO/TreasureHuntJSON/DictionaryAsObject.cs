using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionaryAsObject
{ 
    public List<string> blueCards = new List<string> {}; 
    public List<string> redCards = new List<string> {}; 
    public List<string> neutralCards = new List<string> {}; 
    public List<string> shipwreckCard = new List<string> {};

    public bool isTutorial = false;

    // value populated server side, determines teams
    public int playerIndex = 0;

    public string langaugeSelected = ""; 
}
