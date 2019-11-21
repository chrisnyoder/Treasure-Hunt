using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameCreationScript : MonoBehaviour
{
    GameState initialGameState;
    public BoardLayoutScript boardLayoutScript;
    public List<WordPackProduct> wordPacksToUse = new List<WordPackProduct>{};
    
    public Button generateGameStateButton;
    public GameObject selectAWordPack;

    [HideInInspector]
    public MainBoardNetworkingClient networkingClient; 

    void Start()
    {
        generateGameStateButton = GetComponent<Button>();
    }

    public void generateGameState()
    {
        initialGameState = new GameState(25, wordPacksToUse);


        boardLayoutScript.receiveGameStateObject(initialGameState);

        var initialGameStateAsObject = initialGameState.initialGameStateAsObject;
        networkingClient.sendDictionary(initialGameStateAsObject);

        var storeCanvasAnimator = GameObject.Find("StoreCanvas").GetComponent<Animator>();
        storeCanvasAnimator.Play("StoreCanvasAnimation");
        GlobalAudioScript.Instance.playSfxSound("openMenu");
    }

    public void checkIfAtLeastOneWordPack()
    {
        if(!wordPacksToUse.Any())
        {
            generateGameStateButton.interactable = false;
            selectAWordPack.SetActive(true);
        } else 
        {
            generateGameStateButton.interactable = true;
            selectAWordPack.SetActive(false);
        }
    }

    void Update()
    {

    }
}
