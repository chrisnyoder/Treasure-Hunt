using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CodeProviderHandler : MonoBehaviour
{
    public InputField codeInput;
    public HiddenBoardNetworkingClient hiddenBoardNetworkingClient;
    public GameObject errorMessage;
    public Button confirmCodeButton; 
    public GameObject spinner; 

    private bool searchingForRoom = false;
    private float searchingForRoomTimeOutTimer = 5f;
    private bool searchingForRoomTimedOut = false;

    private string code; 

    private void Awake() {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
    
    void Start()
    {
        confirmCodeButton.interactable = true;
        errorMessage.SetActive(false);
        spinner.SetActive(false);
    }

    private void Update() 
    {
        code = codeInput.text.ToLower();    
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

        if(code.Length != 4)
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
        hiddenBoardNetworkingClient.joinGameWithCode(code);
        searchingForRoom = true;
    }

    public void onJoinedRoom()
    {
        confirmCodeButton.interactable = true;
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(-3000, 1f, false);
        searchingForRoom = false;
        resetSearchingTimer();
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
}
