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
        if(timerStarted && timer < 180)
        {
            timer += Time.deltaTime;
            print(timer);
            float percentTimeLeft = timer / 180;
            print(percentTimeLeft);
            mask.offsetMin = new Vector2(0, percentTimeLeft * parent.sizeDelta.y);
        } else if(timerStarted && timer >= 180) 
        {
            endTurn.changeTurns();
        }
    }
}
