﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum tabs
{
    RedTab,
    BlueTab,
    NeutralTab
}

public class HiddenBoardViewController : MonoBehaviour
{

    public List<string> redWords;
    public List<string> blueWords;
    public List<string> neutralWords;

    private tabs tabSelected = tabs.RedTab;
    private int numberOfWordObjectsToBeCreated;
    private List<string> wordList;

    public Image scrollImage;
    public GameObject collectionView;
    private RectTransform collectionViewRT;
    public GameObject textObject;

    public Sprite blueScrollImage;
    public Sprite redScrollImage;
    public Sprite neutralScrollImage;

    public Button redButton;
    public Button blueButton;
    public Button neutralButton;

    private Vector3 defaultRedButtonPosition;
    private Vector3 defaultBlueButtonPosition;
    private Vector3 defaultNeutralButtonPosition;

    private List<GameObject> textObjects;
    private List<RectTransform> textPositions;

    // Start is called before the first frame update

    public void receiveGameStateObject(GameState initialGameState)
    {
        print("hidden board layout being made");

        redWords = new List<string>() { };
        blueWords = new List<string>() { };
        neutralWords = new List<string>() { };

        foreach (CardObject word in initialGameState.hiddenBoardList)
        {
            if (word.cardType == CardType.redCard)
            {
                redWords.Add(word.labelText);
            }
            else if (word.cardType == CardType.blueCard)
            {
                blueWords.Add(word.labelText);
            }
            else if (word.cardType == CardType.neutralCard)
            {
                neutralWords.Add(word.labelText);
            }
            else
            {
                print("card is the ship wreck card... deal with later");
            }
        }

        numberOfWordObjectsToBeCreated = redWords.Count;
        wordList = redWords;

        textObjects = new List<GameObject>() { };
        textPositions = new List<RectTransform>() { };

        getTextObjectSize();
    }

    void getTextObjectSize()
    {

        collectionViewRT = collectionView.GetComponent<RectTransform>();
        var scrollImageRT = scrollImage.GetComponent<RectTransform>();
        var textObjectRT = textObject.GetComponent<RectTransform>();
        collectionViewRT.sizeDelta = new Vector2(scrollImageRT.rect.width * (float)(0.9), scrollImageRT.rect.height * (float)(0.66));

        textObjectRT.sizeDelta = new Vector2(collectionViewRT.rect.width, collectionViewRT.rect.height / numberOfWordObjectsToBeCreated);

        getInitialButtonPositions();
    }

    void getInitialButtonPositions()
    {
        defaultBlueButtonPosition = blueButton.transform.localPosition;
        defaultRedButtonPosition = redButton.transform.localPosition;
        defaultNeutralButtonPosition = neutralButton.transform.localPosition;
        createTextPrefabs();
    }

    void createTextPrefabs()
    {
        textObject.SetActive(true);

        switch (tabSelected)
        {
            case tabs.RedTab:
                redButton.transform.localPosition = returnPositionOfSelectedTab(defaultRedButtonPosition);
                blueButton.transform.localPosition = defaultBlueButtonPosition;
                neutralButton.transform.localPosition = defaultNeutralButtonPosition;

                scrollImage.sprite = redScrollImage;
                numberOfWordObjectsToBeCreated = redWords.Count;
                wordList = redWords;
                break;
            case tabs.BlueTab:
                redButton.transform.localPosition = defaultRedButtonPosition;
                blueButton.transform.localPosition = returnPositionOfSelectedTab(defaultBlueButtonPosition);
                neutralButton.transform.localPosition = defaultNeutralButtonPosition;

                scrollImage.sprite = blueScrollImage;
                numberOfWordObjectsToBeCreated = blueWords.Count;
                wordList = blueWords;
                break;
            case tabs.NeutralTab:
                redButton.transform.localPosition = defaultRedButtonPosition;
                blueButton.transform.localPosition = defaultBlueButtonPosition;
                neutralButton.transform.localPosition = returnPositionOfSelectedTab(defaultNeutralButtonPosition);

                scrollImage.sprite = neutralScrollImage;
                numberOfWordObjectsToBeCreated = neutralWords.Count;
                wordList = neutralWords;
                break;
        }

        resetTextList();

        for (int n = 0; n < numberOfWordObjectsToBeCreated; ++n)
        {
            GameObject textClone = Instantiate(textObject, new Vector3(0, 0, 0), Quaternion.identity, collectionView.transform);
            var textObjectRT = textClone.GetComponent<RectTransform>();
            textClone.transform.SetParent(collectionView.transform, false);
            textPositions.Add(textObjectRT);

            var textObjectData = textClone.GetComponent<Text>();
            textObjectData.text = wordList[n];
            textObjectData.fontSize = 30;
            textObjects.Add(textClone);
        }

        layoutText(textPositions);
    }

    private Vector3 returnPositionOfSelectedTab(Vector3 oldTabPosition)
    {
        print("old position is: " + oldTabPosition);
        var newTabPosition = oldTabPosition - new Vector3(0f, 50f, 0f);
        print("new tab position is: " + newTabPosition);
        return newTabPosition;
    }

    void layoutText(List<RectTransform> listOfTextRTs)
    {
        float xOrigin = 0;
        float yOrigin = 0;

        var textHeight = collectionViewRT.rect.height / numberOfWordObjectsToBeCreated;
        float verticalSpaceRemaining = collectionViewRT.rect.height;

        print("number of objects in RT list" + listOfTextRTs.Count);

        foreach (RectTransform text in listOfTextRTs)
        {
            text.anchoredPosition = new Vector2(xOrigin, yOrigin);
            yOrigin -= textHeight;
            verticalSpaceRemaining -= textHeight;
        }

        textObject.SetActive(false);
    }

    private void resetTextList()
    {
        if (textObjects.Count > 0)
        {
            foreach (GameObject GO in textObjects)
            {
                Destroy(GO);
            }
        }

        if (textPositions.Count > 0)
        {
            foreach (RectTransform RT in textPositions)
            {
                Destroy(RT);
            }
        }

        textObjects.Clear();
        textPositions.Clear();
    }

    public void selectRedTab()
    {
        tabSelected = tabs.RedTab;
        createTextPrefabs();
    }

    public void selectBlueTab()
    {
        tabSelected = tabs.BlueTab;
        createTextPrefabs();
    }

    public void selectNeutralTab()
    {
        tabSelected = tabs.NeutralTab;
        createTextPrefabs();
    }
}
