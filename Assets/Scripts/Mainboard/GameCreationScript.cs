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
    public CodeDisplayHandler codeDisplayHandler;

    [HideInInspector]
    public MainBoardNetworkingClient networkingClient; 

    private bool restarting = false; 

    void Start()
    {
        generateGameStateButton = GetComponent<Button>();
    }

    public void generateGameState()
    {
        if(initialGameState != null)
        {
            restarting = true; 
        }

        initialGameState = new GameState(25, wordPacksToUse);
        if(restarting)
        {
            initialGameState.currentGameState = CurrentGameState.blueTurn;
            networkingClient.sendCurrentGameState(CurrentGameState.blueTurn);
        } else 
        {
            initialGameState.currentGameState = CurrentGameState.none;
        }

        boardLayoutScript.receiveGameStateObject(initialGameState);

        networkingClient.initialGameState = initialGameState;
        codeDisplayHandler.displayWaitingForGameIndicator();

        var storeCanvasAnimator = GameObject.Find("StoreCanvas").GetComponent<Animator>();
        storeCanvasAnimator.Play("StoreCanvasAnimation");
        GlobalAudioScript.Instance.playSfxSound("openMenu");

        if(GlobalDefaults.Instance.tutorialIsOn) 
        {
            codeDisplayHandler.gameObject.SetActive(false);
        }

        boardLayoutScript.runMainBoardAnimation();
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
}
