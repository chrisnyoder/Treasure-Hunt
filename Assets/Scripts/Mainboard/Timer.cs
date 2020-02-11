using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public RectTransform parent; 
    public RectTransform mask;
    public EndTurnHandler endTurn; 
    public bool timerStarted = false; 
    public bool timerPaused = false; 
    public Image timerImage; 

    public WSNetworkingClient networkingClient;

    public float secondsElapsed = 0; 

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();

        Debug.Log("awake function being called on timer: " + gameObject.name + " " + gameObject.GetInstanceID());
    }

    private void Start()    
    {
        resetTimer();
    }

    public void resetTimer()
    {
        secondsElapsed = 0;
        networkingClient.timerObject.timeTakenOnTurn = "0";
        mask.offsetMin = new Vector2(0, parent.sizeDelta.y);
    }

    public void toggleTimer()
    {
        if(networkingClient.networkedGameState.currentGameState == CurrentGameState.blueTurn || networkingClient.networkedGameState.currentGameState == CurrentGameState.redTurn)
        {
            networkingClient.pauseGame(pauseGameCallback);
            print("timer is being paused");    
        }
    }

    private void pauseGameCallback(JSONObject data)
    {
        print("pause timer callback received");
        if (timerPaused)
        {
            timerPaused = false;
            networkingClient.gamePaused = false;
            timerImage.sprite = Resources.Load<Sprite>("Images/UIElements/hourglass_icon_on");
        }
        else
        {
            timerPaused = true;
            networkingClient.gamePaused = true;
            timerImage.sprite = Resources.Load<Sprite>("Images/UIElements/hourglass_icon_off");
        }
    }

    // Update is called once per frame
    void Update()   
    {
        if (networkingClient.timerObject != null)
        {
            if (Mathf.Abs((float.Parse(networkingClient.timerObject.timeTakenOnTurn) - secondsElapsed)) > 5 && timerStarted && !timerPaused)
            {
                print("timer on server is: " + float.Parse(networkingClient.timerObject.timeTakenOnTurn) + " timer on client is " + secondsElapsed);
                secondsElapsed = float.Parse(networkingClient.timerObject.timeTakenOnTurn);
            }
        };

        if(timerStarted && !timerPaused && secondsElapsed <= 180)
        {
            secondsElapsed += Time.deltaTime;
            float percentTimeLeft = (180 - secondsElapsed) / 180;
            mask.offsetMin = new Vector2(0, parent.sizeDelta.y * percentTimeLeft);
        } else if(timerStarted && secondsElapsed >= 180 && !timerPaused) 
        {
            networkingClient.timerObject.timeTakenOnTurn = "0";

            if(endTurn != null) 
            {
                GlobalAudioScript.Instance.playSfxSound2("Slam_Metal_03");
                endTurn.toggleTurns();
                
            }
        }

        if(timerPaused != networkingClient.gamePaused)
        {
            timerPaused = networkingClient.gamePaused;
        }
    }
}
