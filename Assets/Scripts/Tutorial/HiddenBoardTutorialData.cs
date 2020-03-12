using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBoardTutorialData
{
    private int _screenIndex;
    private Team _team;

    public HiddenBoardTutorialData(int screenNumber, Team team)
    {
        this._team = team;
        this._screenIndex = screenNumber;
    }

    public static int numberOfScreens
    {
        get { return mainTexts.Count; }
    }

    public string titleText
    {
        get { return titleTexts[_screenIndex]; }
    }

    public string mainText
    {
        get
        {
            if (_team == Team.RedTeam)
            {
                return mainTextRed[_screenIndex];
            }
            else
            {
                return mainTexts[_screenIndex];
            }
        }
    }

    private static List<string> mainTexts = new List<string>
    {
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_01"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_02"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_03"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_04"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_05"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_06"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_07"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_08"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_09"),
        LocalizationManager.instance.GetLocalizedText("tutorial_captain_10")
    };

    private static List<string> mainTextRed = new List<string>
    {
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_01"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_02"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_03"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_04"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_05"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_06"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_07"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_08"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_09"),
        LocalizationManager.instance.GetLocalizedText("red_tutorial_captain_10")
    };

    private static List<string> titleTexts = new List<string>
    {
        "Welcome Captain!",
        "",
        "",
        "",
        "",
        ""
    };
}   
