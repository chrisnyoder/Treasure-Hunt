using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBoardTutorialData
{
    private int _screenIndex;
    private Team _team;

    public HiddenBoardTutorialData(int screenNumber, Team team)
    {
        Debug.Log("team is: " + team);
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
        "From this screen you can see which words your team has to guess in order to win the game. You can see the other team’s words as well as the neutral words by clicking the tabs under the scroll.",
        "<color=#00ACB9> Blue </color> team goes first. Select the tab beneath the scroll.",
        "These are the words that your team needs to guess.\nIt's time to give your team a clue",
        "We're going to give you the first clue to get you started.",
        "2 of the words blue team needs to guess are <color=#00ACB9> Saturn </color> and <color=#00ACB9> satellite</color>. Blue Team's captain, go ahead and give your team the clue <color=#00ACB9> space - 2</color>. <2> here refers to the amount of words you want your team to guess",
        "As your team flips over words on the main board, you will see them get crossed out on the scroll. Hopefully, your team found <color=#00ACB9> Saturn </color> and <color=#00ACB9> satellite </color>",
        "In addition to your team's words, you can view the other team's words (as well as neutral words) by selecting the tabs along the bottom",
        "Keep an eye on these words as well. If you flip over a card that is not yours, play passes to the next team",
        "Be careful! If your team selects the word in the danger box the game is over and the other team wins",
        "You can continue this game or exit to start a new one. If you ever need a refresher, the tutorial can be turned back on from the home screen. Have fun!"
    };

    private static List<string> mainTextRed = new List<string>
    {
        "From this screen you can see which words your team has to guess in order to win the game. You can see the other team’s words as well as the neutral words by clicking the tabs under the scroll.",
        "<color=#e03b28> Red </color> team, you go second. Select the red tab underneath the scroll window",
        "These are the words that your team needs to guess. Blue team is going to start. Please wait while they give their team a clue. Once they're done, tap to continue.",
        "<color=#e03b28> Red </color> team goes second, but only has to find 7 coins. Your job is to give clues to your team. We'll give you the first one to get you started.",
        "2 of the words that <color=#e03b28> Red </color> team needs to guess are <color=#e03b28> gold </color> and <color=#e03b28> money </color>. Go and give your team the clue <color=#e03b28> currency -2</color>. <2> here refers to the amount of words you want your team to guess.",
        "As your team flips over words on the main board, you will see them get crossed out on the scroll. Hopefully, your team found <color=#e03b28> gold </color> and <color=#e03b28> money </color>",
        "In addition to your team's words, you can view the other team's words (as well as neutral words) by selecting the tabs along the bottom",
        "Keep an eye on these words as well. If you flip over a card that is not yours, play passes to the next team",
        "Be careful! If your team selects the word in the danger box the game is over and the other team wins",
        "You can continue this game or exit to start a new one. If you ever need a refresher, the tutorial can be turned back on from the home screen. Have fun!"
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
