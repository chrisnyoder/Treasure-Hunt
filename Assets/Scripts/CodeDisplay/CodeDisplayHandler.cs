using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CodeDisplayHandler : MonoBehaviour
{
    
    private GameObject codeDisplayBackground; 
    private string connectionCode = "";

    public GameObject listOfJoinedPlayers;
    public Text gameIdText;
    private Vector2 initialTextBoxSize; 
    public GameObject spinner;
    public GameObject beginButton;
    public GameObject goBackButton;

    private bool codeRecieved = false;
    private float waitingForCodeTimeOutTimer = 6f;
    private bool waitingForCodeTimedOut = false;

    void Start()
    {
        initialTextBoxSize = gameIdText.gameObject.GetComponent<RectTransform>().sizeDelta;
        spinner.SetActive(false);
        beginButton.SetActive(true);
        goBackButton.SetActive(false);
    }

    public void displayWaitingForGameIndicator()
    {
        spinner.SetActive(true);
        var anim = spinner.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -360), 3f, RotateMode.FastBeyond360);
        anim.SetLoops(-1, LoopType.Incremental);
        anim.Play();
    }   

    private void Update() 
    {
        if(codeRecieved)
        {
            hideWaitinForGameIdIndicator();
        } else 
        {
            waitingForCodeTimeOutTimer -= Time.deltaTime;
            if (waitingForCodeTimeOutTimer <= 0)
            {
                waitingForCodeTimedOut = true;
            }
        }

        if(waitingForCodeTimedOut)
        {
            spinner.SetActive(false);
            beginButton.SetActive(false);
            goBackButton.SetActive(true);
            gameIdText.text = "We couldn't get a game ID. This might be a problem with our server or your internet connection";
            gameIdText.gameObject.GetComponent<RectTransform>().sizeDelta = (initialTextBoxSize * 1.5f);
        }
    }

    private void hideWaitinForGameIdIndicator()
    {
        spinner.SetActive(false);
    }

    public void startGame()
    {
        var rt = gameObject.GetComponent<RectTransform>();
        rt.DOAnchorPosY(-2000, 1f, false);
    }

    public void displayConnectionCode(string receivedConnectionCode)
    {
        codeRecieved = true;
        waitingForCodeTimedOut = false;
        beginButton.SetActive(true);
        goBackButton.SetActive(false);
        resetTimeOutTimer();

        connectionCode = receivedConnectionCode;
        gameIdText.gameObject.GetComponent<RectTransform>().sizeDelta = initialTextBoxSize;
        gameIdText.text = ("Game ID: " + receivedConnectionCode);
    }

    private void resetTimeOutTimer()
    {
        waitingForCodeTimeOutTimer = 6f;
    }
}
