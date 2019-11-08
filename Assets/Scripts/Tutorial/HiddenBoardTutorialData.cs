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
        "<color=#00ACB9> Blue </color> team goes first but has to find one extra coin. Your job is to give clues to your team. We're going to give you the first clue to get you started. After this you're on your own!",
        "2 of the words blue team needs to guess are <color=#00ACB9> Saturn </color> and <color=#00ACB9> satellite </color>. Blue Team's captain, go ahead and give your team the clue <color=#00ACB9> space - 2 </color>. <2> here refers to the amount of words you want your team to guess",
        "If, for any reason, your team flips over a neutral word or an opposing team’s word, your turn ends and play passes to the other team",
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
