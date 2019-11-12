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
        get {return referenceCanvases[_screenIndex]; }
    }

    private static List<string> titleTexts = new List<string>
    {
        "Welcome to Treasure Hunt!",
        "4+ players required to play",
        "This is the pack selection screen",
        "This is the Main Board",
        "Divide into 2 teams",
        "Please wait for the team captains"
    };

    private static List<string> mainTexts = new List<string> 
    {
        "We're going to walk you through the basics to get you started", 
        "2 people will play as team captains, giving their teams clues. The other players will try and guess what cards they're supposed to turn over. Don't worry we'll explain later ;)",
        "For your first game we're going to get you started with the Starter word pack",
        "Here is where players will reveal words to see if their team’s coins are hidden underneath",
        "Each team picks a team captain. The captains should open Treasure hunt on their personal devices and select <join game> from the home screen. The captains must keep their screens hidden",
        "the goal of Treasure Hunt is to be the first to find all your team’s coins. Blue team has 8, Red team has 7. The captains will give clues to help you flip over the right words. Good luck and have fun!"
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
        "MainBoardCanvas", 
        "MainBoardCanvas", 
        "MainBoardCanvas"
    };
}
