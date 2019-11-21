using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public Text shipwreckCardText;

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

    public Tween downTween;

    public Vector2 blueButtonInitialPos;
    public Vector2 redButtonInitialPos;
    public Vector2 neutralButtonInitialPos;

    private List<GameObject> textObjects;
    private List<RectTransform> textPositions;
    private List<string> wordsSelected; 

    // Start is called before the first frame update

    private void Start() 
    {
        blueButtonInitialPos = blueButton.GetComponent<RectTransform>().anchoredPosition;
        redButtonInitialPos = redButton.GetComponent<RectTransform>().anchoredPosition;
        neutralButtonInitialPos = neutralButton.GetComponent<RectTransform>().anchoredPosition;

        if(GlobalDefaults.Instance.tutorialIsOn)
        {
            redButton.enabled = false;
            blueButton.enabled = false;
            neutralButton.enabled = false;
        } 
    }

    public void initializeHiddenBoard()
    {   
        
        redWords = new List<string>(){"searching"};
        blueWords = new List<string>(){"searching"};
        neutralWords = new List<string>(){"searching"};
        wordsSelected = new List<string>(){};

        shipwreckCardText.text = "";

        wordList = redWords;
        textObjects = new List<GameObject>() {};
        textPositions = new List<RectTransform>(){};

        getTextObjectSize();
    }

    // entry for recreating text objects on the scroll 
    public void getTextObjectSize()
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
                scrollImage.sprite = redScrollImage;
                numberOfWordObjectsToBeCreated = redWords.Count;
                wordList = redWords;
                break;
            case Tabs.BlueTab:
                scrollImage.sprite = blueScrollImage;
                numberOfWordObjectsToBeCreated = blueWords.Count;
                wordList = blueWords;
                break;
            case Tabs.NeutralTab:
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
            if (!GlobalDefaults.Instance.isTablet)
            {
                textObjectData.resizeTextForBestFit = true;
            }
            textObjectData.text = wordList[n];
            textObjectData.fontSize = 30;
            textObjects.Add(textClone);
        }

        shipwreckCardText.gameObject.GetComponent<RectTransform>().sizeDelta = textObjects[0].GetComponent<RectTransform>().sizeDelta;

        layoutText(textPositions);
    }

    void layoutText(List<RectTransform> listOfTextRTs)
    {
        float xOrigin = 0;
        float yOrigin = 0;

        var textHeight = collectionViewRT.rect.height / numberOfWordObjectsToBeCreated;

        float verticalSpaceRemaining = collectionViewRT.rect.height;


        foreach (RectTransform text in listOfTextRTs)
        {
            text.anchoredPosition = new Vector2(xOrigin, yOrigin);
            yOrigin -= textHeight;
            verticalSpaceRemaining -= textHeight;
        }

        setStrikethroughsOnWords();
        textObject.SetActive(false);
    }

     void animateTab(Tabs tab)
    {
        Tween upTween1 = redButton.GetComponent<RectTransform>().DOAnchorPosY(redButtonInitialPos.y, 0.3f, false); 
        Tween upTween2 = neutralButton.GetComponent<RectTransform>().DOAnchorPosY(neutralButtonInitialPos.y, 0.3f, false);
        Tween downTween = blueButton.GetComponent<RectTransform>().DOAnchorPosY(blueButtonInitialPos.y, 0.3f, false);  

        switch(tab)
        {
            case Tabs.BlueTab:
                upTween1 = redButton.GetComponent<RectTransform>().DOAnchorPosY(redButtonInitialPos.y, 0.3f, false);
                upTween2 = neutralButton.GetComponent<RectTransform>().DOAnchorPosY(neutralButtonInitialPos.y, 0.3f, false);
                downTween = blueButton.GetComponent<RectTransform>().DOAnchorPosY(blueButtonInitialPos.y - 50, 0.3f, false);
                break;
            case Tabs.RedTab:
                upTween1 = blueButton.GetComponent<RectTransform>().DOAnchorPosY(blueButtonInitialPos.y, 0.3f, false);
                upTween2 = neutralButton.GetComponent<RectTransform>().DOAnchorPosY(neutralButtonInitialPos.y, 0.3f, false);
                downTween = redButton.GetComponent<RectTransform>().DOAnchorPosY(redButtonInitialPos.y - 50, 0.3f, false);
                break; 
            case Tabs.NeutralTab:
                upTween1 = redButton.GetComponent<RectTransform>().DOAnchorPosY(redButtonInitialPos.y, 0.3f, false);
                upTween2 = blueButton.GetComponent<RectTransform>().DOAnchorPosY(blueButtonInitialPos.y, 0.3f, false);
                downTween = neutralButton.GetComponent<RectTransform>().DOAnchorPosY(neutralButtonInitialPos.y - 50, 0.3f, false);
                break;
        }

        upTween1.Play();
        upTween2.Play();
        downTween.Play();
    }

    private void setStrikethroughsOnWords()
    {
        foreach (GameObject textObject in textObjects)
        {
            var strikethroughImage = textObject.GetComponentInChildren<Image>();
            var wordOnHiddenboard = textObject.GetComponent<Text>();

            foreach (var word in wordsSelected)
            {
                if (wordOnHiddenboard.text == word)
                {
                    strikethroughImage.enabled = true;
                }
            }
        }
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
        animateTab(Tabs.RedTab);
        createTextPrefabs();
    }

    public void selectBlueTab()
    {
        tabSelected = Tabs.BlueTab;
        animateTab(Tabs.BlueTab);
        createTextPrefabs();
    }

    public void selectNeutralTab()
    {
        tabSelected = Tabs.NeutralTab;
        animateTab(Tabs.NeutralTab);
        createTextPrefabs();
    }

    public void gameStateChanged(CurrentGameState newGameState)
    {
    }

    public void wordSelected(List<string> wordsSelected)
    {
        this.wordsSelected = wordsSelected;
        setStrikethroughsOnWords();
    }

    public void newLanguage()
    {
    }
}
