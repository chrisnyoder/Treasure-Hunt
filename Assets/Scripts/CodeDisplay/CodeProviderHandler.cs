using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CodeProviderHandler : CodeHandlerAbstract
{
    public InputField codeInput;
    public JoinGameNetworkingClient networkingClient;
    public UIManager uIManager;
    
    public GameObject errorMessage;
    private Color errorColor; 
    public Button confirmCodeButton; 
    public GameObject spinner; 
    private Color spinnerColor; 

    private bool searchingForRoom = false;
    private float searchingForRoomTimeOutTimer = 5f;
    private bool searchingForRoomTimedOut = false;
    private bool roomFound; 

    private Scene scene;


    void Start()
    {
        StartCoroutine(waitForOrientationChange());
        confirmCodeButton.interactable = true;
        
        errorColor = errorMessage.GetComponent<Text>().color;
        errorMessage.GetComponent<Text>().color = new Color(errorColor.r, errorColor.g, errorColor.b, 0);

        spinnerColor = spinner.GetComponent<Image>().color;
        spinner.GetComponent<Image>().color = new Color(spinnerColor.r, spinnerColor.g, spinnerColor.b, 0);

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
            string message = LocalizationManager.instance.GetLocalizedText("no_game_id");
            displayErrorMessage(message, true);
        }
    }

    public void validateCode()
    {
        bool codeValid = true;

        if(connectionCode.Length != 4)
        {
            codeValid = false;
        }

        if(codeValid)
        {
            sendCode();
        } else 
        {
            string msg = LocalizationManager.instance.GetLocalizedText("enter_valid_id");
            displayErrorMessage(msg, true);
        }
    }

    private void displayErrorMessage(string msg, bool buttonInteractable)
    {
        confirmCodeButton.interactable = buttonInteractable;
        errorMessage.GetComponent<Text>().text = msg;
        errorMessage.GetComponent<Text>().color = new Color(errorColor.r, errorColor.g, errorColor.b, 1);

        if(buttonInteractable)
        {
            spinner.GetComponent<Image>().color = new Color(spinnerColor.r, spinnerColor.g, spinnerColor.b, 0); ;
        }
    }

    private void sendCode()
    {
        confirmCodeButton.interactable = false;
        networkingClient.joinGameWithCode(connectionCode);
        searchingForRoom = true;
    }

    public void onJoinedRoom(Role role)
    {
        if(!roomFound)
        {
            roomFound = true; 

            confirmCodeButton.interactable = true;
            gameObject.GetComponent<RectTransform>().DOAnchorPosY(-3000, 1f, false);
            searchingForRoom = false;
            resetSearchingTimer();

            switch (role)
            {
                case Role.captain:
                    uIManager.GoToHiddenBoard();
                    break;
                case Role.crew:
                    uIManager.GoToMainBoardAsCrew();
                    break;
            }
        }
    }

    private void displaySearchingGameUI()
    {
        string msg = LocalizationManager.instance.GetLocalizedText("searching_game_id");
        displayErrorMessage(msg, false);
        spinner.GetComponent<Image>().color = new Color(spinnerColor.r, spinnerColor.g, spinnerColor.b, 1); ;
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
    }
}
