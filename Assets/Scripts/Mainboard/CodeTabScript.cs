using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CodeTabScript : MonoBehaviour
{
    private Text connectionCodeText;
    private bool tabIsClosed = true;
    
    [HideInInspector]
    public string connectionCode = "";
    public RectTransform tabTransform;
    public RectTransform textTransform;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        connectionCodeText = gameObject.GetComponentInChildren<Text>();
        connectionCodeText.text = connectionCode;

        if(GlobalDefaults.Instance.isTablet)
        {
            tabTransform.anchorMin = new Vector2(0.2f, 1);
            tabTransform.anchorMax = new Vector2(0.2f, 1);
            tabTransform.localRotation = new Quaternion(0f, 0f, 180f/360f, 0f);
            textTransform.localRotation = new Quaternion(0f, 0f, 180f / 360f, 0f);
            textTransform.anchoredPosition = textTransform.anchoredPosition - new Vector2(0, -20);
            tabTransform.anchoredPosition = new Vector2(0, 0); 
        }
    }

    public void updateConnectionCode(string code)
    {
        connectionCodeText.text = code; 
    }

    public void showTab()
    {
        gameObject.SetActive(true);

        if(GlobalDefaults.Instance.isTablet)
        {
            if (tabTransform.anchoredPosition.y >= 0)
                tabTransform.DOAnchorPos3DY(tabTransform.anchoredPosition.y - (0.25f * tabTransform.rect.height), 0.5f, false);
        } else 
        {
            if (tabTransform.anchoredPosition.x <= 0)
                tabTransform.DOAnchorPos3DX(tabTransform.anchoredPosition.x + (0.25f * tabTransform.rect.width), 0.5f, false);
        }
    }

    public void toggleTab()
    {
        GlobalAudioScript.Instance.playSfxSound("togglePack2");

        if(GlobalDefaults.Instance.isTablet)
        {
            if (tabIsClosed)
            {
                tabTransform.DOAnchorPos3DY(tabTransform.anchoredPosition.y - (0.3f * tabTransform.rect.height), 0.5f, false);
                tabIsClosed = false;
            }
            else
            {
                tabTransform.DOAnchorPos3DY(tabTransform.anchoredPosition.y + (0.3f * tabTransform.rect.height), 0.5f, false);
                tabIsClosed = true;
            }
        } else 
        {
            if (tabIsClosed)
            {
                tabTransform.DOAnchorPos3DX(tabTransform.anchoredPosition.x + (0.49f * tabTransform.rect.width), 0.5f, false);
                tabIsClosed = false;
            }
            else
            {
                tabTransform.DOAnchorPos3DX(tabTransform.anchoredPosition.x - (0.49f * tabTransform.rect.width), 0.5f, false);
                tabIsClosed = true;
            }
        }
    }
}
