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
                endTurn.toggleTurns();
            }
        }

        if(timerPaused != networkingClient.gamePaused)
        {
            timerPaused = networkingClient.gamePaused;
        }
    }
}
