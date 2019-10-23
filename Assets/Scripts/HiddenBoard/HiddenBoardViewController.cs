using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Tabs
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

    private Tabs tabSelected = Tabs.RedTab;
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

    public Text shipwreckCardText;

    private Vector3 defaultRedButtonPosition;
    private Vector3 defaultBlueButtonPosition;
    private Vector3 defaultNeutralButtonPosition;

    private List<GameObject> textObjects;
    private List<RectTransform> textPositions;

    // Start is called before the first frame update


    public void receiveMainBoardDictionary(Dictionary<CardType, List<string>> WordListDictionary)
    {
        print("hidden dictionary has been received");

        foreach(KeyValuePair<CardType, List<string>> entry in WordListDictionary)
        {
            print(entry.Key);
            print(entry.Value);
        }

        foreach (KeyValuePair<CardType, List<string>> entry in WordListDictionary)
        {
            if (entry.Key == CardType.redCard)
            {
                redWords = entry.Value;
            }
            else if (entry.Key == CardType.blueCard)
            {
                blueWords = entry.Value;
            }
            else if (entry.Key == CardType.neutralCard)
            {
                neutralWords = entry.Value;
            }   
            else if(entry.Key == CardType.shipwreckCard)
            {
                shipwreckCardText.text = entry.Value[0];
            }
        }
        
        getTextObjectSize();
    }

    public void initializeHiddenBoard()
    {   
        
        redWords = new List<string>(){"searching"};
        blueWords = new List<string>(){"searching"};
        neutralWords = new List<string>(){"searching"};

        wordList = redWords;
        textObjects = new List<GameObject>() { };
        textPositions = new List<RectTransform>() { };

        defaultBlueButtonPosition = blueButton.transform.localPosition;
        defaultRedButtonPosition = redButton.transform.localPosition;
        defaultNeutralButtonPosition = neutralButton.transform.localPosition;

        getTextObjectSize();
    }

    void getTextObjectSize()
    {

        switch(tabSelected)
        {
            case Tabs.RedTab:
                numberOfWordObjectsToBeCreated = redWords.Count;
                break;
            case Tabs.BlueTab:
                numberOfWordObjectsToBeCreated = blueWords.Count;
                break;
            case Tabs.NeutralTab: 
                numberOfWordObjectsToBeCreated = neutralWords.Count;
                break;
        }

        collectionViewRT = collectionView.GetComponent<RectTransform>();
        var scrollImageRT = scrollImage.GetComponent<RectTransform>();
        var textObjectRT = textObject.GetComponent<RectTransform>();

        var textObjectHeight = collectionViewRT.rect.height / 7;
        if(redWords.Count > 1)
        {
            textObjectHeight = collectionViewRT.rect.height / numberOfWordObjectsToBeCreated;      
        }  
    
        textObjectRT.sizeDelta = new Vector2(collectionViewRT.rect.width, textObjectHeight);

        createTextPrefabs();

    }

    void createTextPrefabs()
    {
        textObject.SetActive(true);

        switch (tabSelected)
        {
            case Tabs.RedTab:
                redButton.GetComponent<Animator>().Play("RedTabAnimationDown");
                blueButton.GetComponent<Animator>().Play("BlueTabAnimationUp");
                neutralButton.GetComponent<Animator>().Play("YellowTabAnimationUp");

                scrollImage.sprite = redScrollImage;
                numberOfWordObjectsToBeCreated = redWords.Count;
                wordList = redWords;
                break;
            case Tabs.BlueTab:
                redButton.GetComponent<Animator>().Play("RedTabAnimationUp");
                blueButton.GetComponent<Animator>().Play("BlueTabAnimationDown");
                neutralButton.GetComponent<Animator>().Play("YellowTabAnimationUp");

                scrollImage.sprite = blueScrollImage;
                numberOfWordObjectsToBeCreated = blueWords.Count;
                wordList = blueWords;
                break;
            case Tabs.NeutralTab:
                redButton.GetComponent<Animator>().Play("RedTabAnimationUp");
                blueButton.GetComponent<Animator>().Play("BlueTabAnimationUp");
                neutralButton.GetComponent<Animator>().Play("YellowTabAnimationDown");


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
            print("text obect text is: " + textObjectData.text);
            textObjectData.fontSize = 30;
            textObjects.Add(textClone);
        }

        layoutText(textPositions);
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
        tabSelected = Tabs.RedTab;
        createTextPrefabs();
    }

    public void selectBlueTab()
    {
        tabSelected = Tabs.BlueTab;
        createTextPrefabs();
    }

    public void selectNeutralTab()
    {
        tabSelected = Tabs.NeutralTab;
        createTextPrefabs();
    }

    public void gameStateChanged(CurrentGameState newGameState)
    {
        print("some logic here about how to handle the EOG screen");
    }

    public void wordSelected(Dictionary<string, bool> wordSelected)
    {
        print("some logic about how to handle selected words");
    }

    public void newLanguage()
    {
        print("some logic about how to handle language change");
    }
}
