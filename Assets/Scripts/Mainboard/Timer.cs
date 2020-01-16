using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public RectTransform parent; 
    public RectTransform mask;
    public bool timerStarted = false; 
    public EndTurnHandler endTurn; 

    public MainBoardNetworkingClient networkingClient;

    public float timer = 0; 

    private void Start() 
    {
        resetTimer();
    }

    public void resetTimer()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(networkingClient.timerObject != null)
        {
            timer = float.Parse(networkingClient.timerObject.timeTakenOnTurn);
        };

        if(timerStarted && timer <= 180)
        {
            float percentTimeLeft = timer / 180;

            print("perent time left: " + percentTimeLeft);
            print("offset being created: " + parent.sizeDelta.y * percentTimeLeft);
            mask.offsetMin = new Vector2(0, parent.sizeDelta.y * percentTimeLeft);
        } else if(timerStarted && timer >= 180) 
        {
            endTurn.changeTurns();
        }
    }
}
