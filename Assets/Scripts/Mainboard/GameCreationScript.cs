using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameCreationScript : MonoBehaviour
{
    GameState initialGameState;
    public BoardLayoutScript boardLayoutScript;
    public Network network;

    void Start()
    {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();
       
        initialGameState = new GameState(25);
        network.networkInitialGameState(initialGameState);
        network.setNetworkAsServer();
        boardLayoutScript.receiveGameStateObject(initialGameState);
    }
}
