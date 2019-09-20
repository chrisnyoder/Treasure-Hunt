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
        // network = GameObject.Find("NetworkManager").GetComponent<Network>();
        // network.netWorkInitialGameState(initialGameState, true);
    }

    public void generateGameState()
    {

        if(!wordPacksToUse.Any())
        {
            print("No word lists to use: " + wordPacksToUse.Count);
        } else 
        {
            print("generate game state: " + wordPacksToUse.Count);

            var storeCanvasRT = GameObject.Find("StoreCanvas").GetComponent<RectTransform>();
            storeCanvasRT.localPosition = new Vector3(storeCanvasRT.localPosition.x, -1500, 0);

            initialGameState = new GameState(25, wordPacksToUse);
            boardLayoutScript.receiveGameStateObject(initialGameState);
        }
    }

    void Update()
    {

    }
}
