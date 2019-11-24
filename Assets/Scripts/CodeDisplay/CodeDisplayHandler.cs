using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CodeDisplayHandler : MonoBehaviour
{
    
    private GameObject codeDisplayBackground; 
    private string connectionCode = "";
    private bool codeRecieved = false;

    public GameObject listOfJoinedPlayers;
    public Text gameIdText;

    void Start()
    {
    
    }

    public void startGame()
    {
        var rt = gameObject.GetComponent<RectTransform>();
        rt.DOAnchorPosY(-2000, 1f, false);
    }

    public void displayConnectionCode(string receivedConnectionCode)
    {
        codeRecieved = true;
        connectionCode = receivedConnectionCode;
        print("connection code received by handler: " + receivedConnectionCode);
        gameIdText.text = ("Game ID: " + receivedConnectionCode);
    }
}
