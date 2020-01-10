using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using System.IO;

public enum CurrentGameState
{
    blueWins,
    redWins,
    loses, 
    redTurn, 
    blueTurn
}

public class CurrentGameStateAsObject
{
    public CurrentGameState currentGameState;

    public CurrentGameStateAsObject(CurrentGameState currentGameState) {
        this.currentGameState = currentGameState;
    }
}

public class GameState
{
    public List<CardObject> hiddenBoardList;
    List<string> wordList = new List<string>(){};
    public List<string> wordsAlreadySelected = new List<string>(){};
    int numberOfCards;
    int numberOfRedCards;
    int numberOfBlueCards;
    int numberOfNeutralCards;
    int numberOfShipWreckCards;

    public int redTeamScore = 0;
    public int blueTeamScore = 0;
    public GameObject eogCanvas;

    public bool isTutorial; 
    public int playerIndex;  

    public CurrentGameState currentGameState = CurrentGameState.blueTurn;

    public GameState(int numberOfCards, List<WordPackProduct> wordPacksToUse)
    {
        isTutorial = GlobalDefaults.Instance.tutorialIsOn;
        this.numberOfCards = numberOfCards;

        if (numberOfCards == 25)
        {
            numberOfRedCards = 7;
            numberOfBlueCards = 8;
            numberOfNeutralCards = 9;
            numberOfShipWreckCards = 1;
        }


        GetWordList(wordPacksToUse);
    }

    void GetWordList(List<WordPackProduct> wordPacksToUse)
    {
        wordList.Clear();

        if(GlobalDefaults.Instance.tutorialIsOn)
        {
            TextAsset textFile = Resources.Load<TextAsset>("WordLists/tutorialWordList");
            var wordsAsList = ReadLinesFromTextFile(textFile).ToList();
            wordList = wordList.Concat(wordsAsList).ToList();
        } else 
        {
            foreach (WordPackProduct wordPack in wordPacksToUse)
            {
                TextAsset textFile = Resources.Load<TextAsset>("WordLists/" + wordPack.wordPackProductIdentifier);
                var wordsAsList = ReadLinesFromTextFile(textFile).ToList();
                wordList = wordList.Concat(wordsAsList).ToList();
            }

            wordList.Shuffle();
        }
        CreateHiddenBoard();
    }

    public IEnumerable<string> ReadLinesFromTextFile(TextAsset textFile)
    {
        using (StreamReader sr = new StreamReader(new MemoryStream(textFile.bytes)))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }

    void CreateHiddenBoard()
    {
        hiddenBoardList = new List<CardObject>();
        var wordListIndex = 0;

        for (int n = 0; n < numberOfRedCards; ++n)
        {
            hiddenBoardList.Add(new CardObject(CardType.redCard, wordList[wordListIndex]));
            wordListIndex += 1;
        }

        for (int n = 0; n < numberOfBlueCards; ++n)
        {
            hiddenBoardList.Add(new CardObject(CardType.blueCard, wordList[wordListIndex]));
            wordListIndex += 1;
        }

        for (int n = 0; n < numberOfNeutralCards; ++n)
        {
            hiddenBoardList.Add(new CardObject(CardType.neutralCard, wordList[wordListIndex]));
            wordListIndex += 1;
        }

        for (int n = 0; n < numberOfShipWreckCards; ++n)
        {
            hiddenBoardList.Add(new CardObject(CardType.shipwreckCard, wordList[wordListIndex]));
            wordListIndex += 1;
        }
        hiddenBoardList.Shuffle();

    }

    public void LaunchEOGScreen()
    {
        eogCanvas = GameObject.Find("ResultsCanvas");
        var script = eogCanvas.GetComponent<EoGScript>();
        script.DisplayEOGCanvas(this.currentGameState);
    }
}
