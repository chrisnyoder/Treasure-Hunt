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
        if (timer.timerStarted)
        {
            timer.timerStarted = false;
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UIElements/pause_icon_selected");
        }
        else
        {
            timer.timerStarted = true;
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UIElements/pause_icon");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
