using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameCreationScript : MonoBehaviour
{
    GameState initialGameState;
    public BoardLayoutScript boardLayoutScript;
    public List<WordPackProduct> wordPacksToUse = new List<WordPackProduct>{};
    public Network network;

    void Start()
    {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();
    }

    public void generateGameState()
    {

        if(!wordPacksToUse.Any())
        {
            print("No word lists to use, put in error message");
        } else 
        {
            // var storeCanvasRT = GameObject.Find("StoreCanvas").GetComponent<RectTransform>();
            // storeCanvasRT.localPosition = new Vector3(storeCanvasRT.localPosition.x, -1500, 0); 

            initialGameState = new GameState(25, wordPacksToUse);
            network.networkInitialGameState(initialGameState);
            network.setNetworkAsServer();
            boardLayoutScript.receiveGameStateObject(initialGameState);

            var storeCanvasAnimator = GameObject.Find("StoreCanvas").GetComponent<Animator>();
            storeCanvasAnimator.Play("StoreCanvasAnimation");

        }
    }

    public void GameStateCreated()
    {
    }

    void Update()
    {

    }
}
