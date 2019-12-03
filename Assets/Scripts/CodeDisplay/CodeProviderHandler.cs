using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CodeProviderHandler : CodeHandlerAbstract
{
    public InputField codeInput;
    public WSNetworkingClient networkingClient;
    public TutorialHiddenBoardScript tutorialHiddenBoardScript;
    public GameObject mainBoard;
    
    public GameObject errorMessage;
    public Button confirmCodeButton; 
    public GameObject spinner; 
    public GameObject transitionImage; 

    private bool searchingForRoom = false;
    private float searchingForRoomTimeOutTimer = 5f;
    private bool searchingForRoomTimedOut = false;

    private Scene scene;

    [HideInInspector]
    public bool mainBoardRunningTutorial = false; 

    private void Awake() 
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        transitionImage.SetActive(true);
    }
    
    void Start()
    {
        StartCoroutine(waitForOrientationChange());
        confirmCodeButton.interactable = true;
        errorMessage.SetActive(false);
        spinner.SetActive(false);

        scene = SceneManager.GetActiveScene();
    }

    private void Update() 
    {
        connectionCode = codeInput.text.ToLower();    
        if(searchingForRoom)
        {
            searchingForRoomTimeOutTimer -= Time.deltaTime;
            if(searchingForRoomTimeOutTimer <= 0)
            {
                searchingForRoomTimedOut = true;
            }
        }

        if(searchingForRoomTimeOutTimer <= 4f)
        {
            displaySearchingGameUI();
        }

        if(searchingForRoomTimedOut)
        {
            searchingForRoom = false;
            searchingForRoomTimedOut = false;
            resetSearchingTimer();
            string message = "could not find room";
            displayErrorMessage(message, true);
        }
    }

    public void validateCode()
    {
        bool codeValid = true;
        errorMessage.SetActive(false);

        if(connectionCode.Length != 4)
        {
            codeValid = false;
        }

        if(codeValid)
        {
            sendCode();
        } else 
        {
            string msg = "enter valid 4 digit code";
            displayErrorMessage(msg, true);
        }
    }

    private void displayErrorMessage(string msg, bool buttonInteractable)
    {
        confirmCodeButton.interactable = buttonInteractable;
        errorMessage.GetComponent<Text>().text = msg;
        errorMessage.SetActive(true);

        if(buttonInteractable)
        {
            spinner.SetActive(false);
        }
    }

    private void sendCode()
    {
        confirmCodeButton.interactable = false;
        networkingClient.joinGameWithCode(connectionCode);
        searchingForRoom = true;
    }

    public void onJoinedRoom(Team team)
    {
        confirmCodeButton.interactable = true;
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(-3000, 1f, false);
        searchingForRoom = false;
        resetSearchingTimer();

        if(mainBoardRunningTutorial && scene.name == "HiddenBoardScene")
        {
            tutorialHiddenBoardScript.beginTutorial(team);
        }

        if(scene.name == "JoinMainBoardContainer")
        {
            if(mainBoard != null) 
            {
                mainBoard.GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f, false).Play();
            }
        }
    }

    private void displaySearchingGameUI()
    {
        string msg = "searching for game id";
        displayErrorMessage(msg, false);
        spinner.SetActive(true);
        var anim = spinner.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -360), 3f, RotateMode.FastBeyond360);
        anim.SetLoops(-1, LoopType.Incremental);
        anim.Play();
    }

    private void resetSearchingTimer()
    {
        searchingForRoomTimeOutTimer = 5f;
    }


    IEnumerator waitForOrientationChange()
    {
        yield return new WaitForSeconds(0.1f);
        startTransition();
    }
    
    private void startTransition()
    {
        transitionImage.GetComponent<Image>().DOFade(0f, 0.2f).Play();
    }
}
