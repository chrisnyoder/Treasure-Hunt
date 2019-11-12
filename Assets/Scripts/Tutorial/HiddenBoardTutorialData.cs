using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBoardTutorialData
{
    private int _screenIndex;

    public HiddenBoardTutorialData(int screenNumber)
    {
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
        get { return mainTexts[_screenIndex]; }
    }

    private static List<string> mainTexts = new List<string>
    {
        "From this screen you can see which words your team has to guess in order to win the game. You can see the other team’s words as well as the neutral words by clicking the tabs under the scroll.",
        "<color=#00ACB9> Blue </color> team goes first. Select the tab beneath the scroll.",
        "These are the words that your team needs to guess.\nIt's time to give your team a clue",
        "We're going to give you the first clue to get you started.",
        "2 of the words blue team needs to guess are <color=#00ACB9> Saturn </color> and <color=#00ACB9> satellite</color>. Blue Team's captain, go ahead and give your team the clue <color=#00ACB9> space - 2</color>. <2> here refers to the amount of words you want your team to guess",
        "As your team flips over words on the main board, you will see them get crossed out on the scroll. Hopefully, your team found <color=#00ACB9> Saturn </color> and <color=#00ACB9> satellite </color>.",
        "In addition to your team's words, you can view the other team's words (as well as neutral words) by selecting the tabs along the bottom",
        "During the course of a game you should keep an eye on these words as well. If you flip over a card that is not yours, play automatically passes to the next team",
        "Be careful! if your team selects the word in the danger box the game is over and the other team wins",
        "You can continue this game or exit to start a new one. If you ever need a refresher, the rules are available from the home screen. Have fun!"
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
