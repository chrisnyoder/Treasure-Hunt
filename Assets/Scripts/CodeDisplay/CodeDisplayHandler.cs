using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CodeDisplayHandler : CodeHandlerAbstract
{
    private GameObject codeDisplayBackground; 
    private bool _tutorialIsOn; 

    public GameObject listOfJoinedPlayersDisplay;
    private List<GameObject> listOfJoinedPlayerObjects = new List<GameObject> {};
    public GameObject playerTag; 

    public Text gameIdText;
    private Vector2 initialTextBoxSize; 
    public GameObject spinner;
    public GameObject beginButton;
    public GameObject goBackButton;

    public bool codeRecieved = false;
    private float waitingForCodeTimeOutTimer;
    private bool waitingForCodeTimedOut = false;

    void Start()
    {
        beginButton.SetActive(false);
        checkIfTutorialOn();
        initialTextBoxSize = gameIdText.gameObject.GetComponent<RectTransform>().sizeDelta;
        playerTag.SetActive(false);
        setTimeOutTimer();
        spinner.SetActive(false);
        setBeginGameButtonToActive();
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
            beginButton.SetActive(true);
            hideWaitingForGameIdIndicator();
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
            goBackButton.SetActive(true);
            gameIdText.text = LocalizationManager.instance.GetLocalizedText("error_fetching_game_id");
            gameIdText.gameObject.GetComponent<RectTransform>().sizeDelta = (initialTextBoxSize * 1.5f);
        }
    }

    private void hideWaitingForGameIdIndicator()
    {
        spinner.SetActive(false);
    }

    public void displayConnectionCode(string receivedConnectionCode)
    {
        codeRecieved = true;
        waitingForCodeTimedOut = false;
        setBeginGameButtonToActive();
        goBackButton.SetActive(false);
        setTimeOutTimer();

        connectionCode = receivedConnectionCode;
        gameIdText.gameObject.GetComponent<RectTransform>().sizeDelta = initialTextBoxSize;

        gameIdText.text = (LocalizationManager.instance.GetLocalizedText("game_id") + " " + receivedConnectionCode);

    }

    private void setTimeOutTimer()
    {
        waitingForCodeTimeOutTimer = 6f;
    }

    public void displayPlayersInGame(List<string> playersInGame)
    {

        foreach(GameObject go in listOfJoinedPlayerObjects)
        {
            Destroy(go);
        }

        for(var i = 1; i <= playersInGame.Count; ++i)
        {
            var playerTagClone = GameObject.Instantiate(playerTag, listOfJoinedPlayersDisplay.transform);
            playerTagClone.SetActive(true);
            string[] playerJoinedStrings = LocalizationManager.instance.GetLocalizedText("player_joined").Split();
            playerTagClone.GetComponent<Text>().text = (playerJoinedStrings[0] + " " + i + " " + playerJoinedStrings[1]);

            if (LocalizationManager.instance.language == SystemLanguage.Japanese)
            {
                playerTagClone.GetComponent<Text>().font = Resources.Load<Font>("Fonts/NotoSansJP-Bold");
            }
           
            listOfJoinedPlayerObjects.Add(playerTagClone);
        }
    }

    private void checkIfTutorialOn()
    {
        if(GlobalDefaults.Instance.tutorialIsOn)
            _tutorialIsOn = true;
    }

    private void setBeginGameButtonToActive()
    {
        if(!GlobalDefaults.Instance.tutorialIsOn && codeRecieved)
            beginButton.SetActive(true);
    }
}
