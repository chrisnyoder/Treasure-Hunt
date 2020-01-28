﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDefaults : MonoBehaviour
{

    private static GlobalDefaults _instance; 
    private static bool globalSettingsSet = false;

    public static GlobalDefaults Instance  { get { return _instance; } }

    public RectTransform screenSize; 
    public bool isTablet; 
    public bool tutorialIsOn; 

    public int appOpenCounter { get { return _appOpenCounter; } }
    private int _appOpenCounter;

    private void Awake() 
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

            if (!globalSettingsSet)
            {
                setPlayerDefaults();
                setFrameRate();
                determineIfTablet();
                increcmentAppCounter();
                keepScreenOn();
            }
            globalSettingsSet = true;

            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update

    void setPlayerDefaults()
    {
        // PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey("appHasStartedBefore"))
        {
            print("app has started before");

            var allWordPackIds = WordPackProductIdentifiers.returnAllProucts();
            tutorialIsOn = false;

            foreach (string wp in allWordPackIds)
            {
                PlayerPrefs.SetString(wp, PlayerPrefs.GetString(wp));
            }
        }
        else
        {
            PlayerPrefs.SetString("appHasStartedBefore", "true");
            print("app is starting up for first time");

            PlayerPrefs.SetString("initialWordList", "enabled");
            PlayerPrefs.SetString("initialWordListExpansion", "unpurchased");
            PlayerPrefs.SetString("fantasyWordList", "unpurchased");
            PlayerPrefs.SetString("celebritiesWordList", "unpurchased");

            PlayerPrefs.SetString("initialWordListJP", "disabled");
            PlayerPrefs.SetString("initialWordListExpansionJP", "unpurchased");
            
            tutorialIsOn = true;
        }
    }

    void setFrameRate()
    {
        Application.targetFrameRate = 60;
    }

    void keepScreenOn()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void determineIfTablet()
    {
        var screenWidth = screenSize.rect.width;
        var screenHeight = screenSize.rect.height; 

        if(screenWidth < screenHeight)
        {
            screenHeight = screenSize.rect.width;
            screenWidth = screenSize.rect.height;
        }

        var aspectRatio = screenWidth/screenHeight;

        if(aspectRatio <= 1.4)
        {
            isTablet = true;    
        } 
        else
        {
            isTablet = false;
        }
    }

    void increcmentAppCounter() 
    {
        if (PlayerPrefs.HasKey("appOpenCounter"))
        {
            string previousCounter = PlayerPrefs.GetString("appOpenCounter");
            int newCounter = int.Parse(previousCounter) + 1;
            _appOpenCounter = newCounter;

            PlayerPrefs.SetString("appOpenCounter", newCounter.ToString());
        }
        else
        {
            PlayerPrefs.SetString("appOpenCounter", "0");
            _appOpenCounter = 0;
        }
    }
}
