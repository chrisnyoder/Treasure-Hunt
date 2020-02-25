using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialMainScreenData
{
    private int _screenIndex;

    public TutorialMainScreenData(int screenNumber)
    {
        this._screenIndex = screenNumber;
    }

    public static int numberOfScreens
    {
        get { return titleTexts.Count; }
    }

    public string titleText
    {
        get { return titleTexts[_screenIndex]; }
    }
    
    public string mainText
    {
        get { return mainTexts[_screenIndex]; }
    }

    public Vector3 circlePlacement
    {
        get { return circlePlacements[_screenIndex]; }
    }

    public string referenceCanvas
    {
        get { return referenceCanvases[_screenIndex]; }
    }

    private static List<string> titleTexts = new List<string>
    {
        LocalizationManager.instance.GetLocalizedText("tutorial_title_01"),
        LocalizationManager.instance.GetLocalizedText("tutorial_title_02"),
        LocalizationManager.instance.GetLocalizedText("tutorial_title_03"),
        "",
        LocalizationManager.instance.GetLocalizedText("tutorial_title_04"),
        LocalizationManager.instance.GetLocalizedText("tutorial_title_05"),
        LocalizationManager.instance.GetLocalizedText("tutorial_title_06"),
        LocalizationManager.instance.GetLocalizedText("tutorial_title_07")
    };

    private static List<string> mainTexts = new List<string> 
    {
        LocalizationManager.instance.GetLocalizedText("tutorial_01"),
        LocalizationManager.instance.GetLocalizedText("tutorial_02"),
        LocalizationManager.instance.GetLocalizedText("tutorial_03"),
        LocalizationManager.instance.GetLocalizedText("tutorial_04"),
        "",
        LocalizationManager.instance.GetLocalizedText("tutorial_05"),
        LocalizationManager.instance.GetLocalizedText("tutorial_06"),
        LocalizationManager.instance.GetLocalizedText("tutorial_07")
    }; 

    private static List<Vector3> circlePlacements = new List<Vector3>
    {
        new Vector3(785, 130, 0),
        new Vector3(785, 130, 0),
        new Vector3(785, 130, 0),
        new Vector3(785, 130, 0),
        new Vector3(785, 130, 0),
        new Vector3(785, 130, 0)
    };

    private static List<string> referenceCanvases = new List<string>
    {
        "StoreCanvas", 
        "StoreCanvas",
        "StoreCanvas", 
        "StoreCanvas",
        "MainBoardCanvas",
        "MainBoardCanvas", 
        "MainBoardCanvas", 
        "MainBoardCanvas"
    };
}
