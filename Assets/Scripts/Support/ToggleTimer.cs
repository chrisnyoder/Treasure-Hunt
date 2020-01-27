using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTimer : MonoBehaviour
{
    WSNetworkingClient networkingClient;
    public Timer timer; 

    private void Awake()
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    public void toggleTimer()
    {
        networkingClient.pauseGame(pauseGameCallback);
    }

    private void pauseGameCallback(JSONObject data)
    {
        if(timer.timerPaused)
        {
            timer.timerPaused = false;
            networkingClient.gamePaused = false; 
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UIElements/pause_icon");
        }
        else
        {
            timer.timerPaused = true;
            networkingClient.gamePaused = true; 
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UIElements/pause_icon_selected");
        }
    }
}
