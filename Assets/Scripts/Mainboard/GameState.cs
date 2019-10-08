﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;
using System.IO;

public enum CurrentGameState
{
    gameInPlay,
    connecting,
    blueWins,
    redWins,
    loses
}

public class GameState
{
    public List<CardObject> hiddenBoardList;
    List<string> wordList = new List<string>(){};
    int numberOfCards;
    int numberOfRedCards;
    int numberOfBlueCards;
    int numberOfNeutralCards;
    int numberOfShipWreckCards;

    public int redTeamScore = 0;
    public int blueTeamScore = 0;
    public GameObject eogCanvas;

    public CurrentGameState currentGameState;

    public GameState(int numberOfCards)
    {


        this.numberOfCards = numberOfCards;

        if (numberOfCards == 25)
        {
            numberOfRedCards = 7;
            numberOfBlueCards = 8;
            numberOfNeutralCards = 9;
            numberOfShipWreckCards = 1;
        }

        eogCanvas = GameObject.Find("ResultsCanvas");

        GetWordList();
    }

    void GetWordList()
    {
        wordList.Clear();


        TextAsset textFile = Resources.Load<TextAsset>("initialWordList");
        var wordsAsList = ReadLinesFromTextFile(textFile).ToList();
        wordList = wordList.Concat(wordsAsList).ToList();
          

        wordList.Shuffle();

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
        Debug.Log("hidden board is created. It's size is: " + hiddenBoardList.Count);
    }

}
