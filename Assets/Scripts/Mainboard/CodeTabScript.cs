using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CodeTabScript : MonoBehaviour
{
    public Text connectionCodeText;
    private bool tabIsClosed = true;
    
    [HideInInspector]
    public RectTransform tabTransform;
    public RectTransform textTransform;

    // Start is called before the first frame update
    void Start()
    {
        connectionCodeText = gameObject.GetComponent<Text>();
        connectionCodeText.text = "";
    }

    public void updateConnectionCode(string code)
    {
        connectionCodeText.text = code; 
    }

    public void displayRoomId()
    {
        print("displaying room");
        gameObject.SetActive(true);
    }
}
