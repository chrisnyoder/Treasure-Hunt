using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public RectTransform parent; 
    public RectTransform mask;
    public bool timerStarted = false; 

    public WSNetworkingClient networkingClient;

    public float secondsElapsed = 0; 

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();    
    }

    private void Start() 
    {
        resetTimer();
    }

    public void resetTimer()
    {
        if(networkingClient != null)
        {
            secondsElapsed = float.Parse(networkingClient.timerObject.timeTakenOnTurn); 
        } else 
        {
            secondsElapsed = 0;
        }
    }

    // Update is called once per frame
    void Update()   
    {
        if (networkingClient.timerObject != null)
        {
            if (Mathf.Abs((float.Parse(networkingClient.timerObject.timeTakenOnTurn) - secondsElapsed)) > 16 && timerStarted)
            {
                print("timer on server is: " + float.Parse(networkingClient.timerObject.timeTakenOnTurn) + " timer on client is " + secondsElapsed);
                secondsElapsed = float.Parse(networkingClient.timerObject.timeTakenOnTurn);
            }
        };

        if(timerStarted && secondsElapsed <= 180)
        {
            secondsElapsed += Time.deltaTime;
            float percentTimeLeft = (180 - secondsElapsed) / 180;
            mask.offsetMin = new Vector2(0, parent.sizeDelta.y * percentTimeLeft);
        } else if(timerStarted && secondsElapsed >= 180) 
        {
            timerStarted = false;
            resetTimer();
            networkingClient.timerObject.timeTakenOnTurn = "0";
        }
    }
}
