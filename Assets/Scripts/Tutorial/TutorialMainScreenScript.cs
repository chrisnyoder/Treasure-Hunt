using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class TutorialMainScreenScript : MonoBehaviour
{        
    private int tutorialIndexNumber = 0; 
    private int numberOfTutorialScreens;

    private Vector2 initialCircleImageSize; 
    private Vector3 initialCircleImagePos; 
    private Vector3 initialVerticalLayoutGroupPos; 
    private Vector2 initialMinAnchorsForImageCircle;
    private Vector2 initialMaxAnchorsForImageCircle;

    public GameObject storeCollectionView;  
    private Vector2 initialMinAnchorsForStarterPack;
    private Vector2 initialMaxAnchorsForStarterPack;

    public Text titleText;
    public Text mainText; 
    public Text continueText; 
    public GameObject exitTutorialButton; 

    public GameObject tutorialCircleImage;
    public GameObject verticalLayoutGroup;  

    private RectTransform tutorialCircleImageRT; 
    private RectTransform verticalLayoutGroupRT; 

    public GameObject groupImage; 
    public GameObject gameCreation;
    public GameObject currentCanvas; 

    public GameObject backgroundCanvas;  

    void Start()
    {
        groupImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        tutorialCircleImageRT = tutorialCircleImage.GetComponent<RectTransform>();
        verticalLayoutGroupRT = verticalLayoutGroup.GetComponent<RectTransform>();    

        initialCircleImageSize = tutorialCircleImageRT.sizeDelta;
        initialCircleImagePos = tutorialCircleImageRT.anchoredPosition3D;
        initialVerticalLayoutGroupPos = verticalLayoutGroupRT.anchoredPosition3D;

        initialMinAnchorsForImageCircle = tutorialCircleImageRT.anchorMin;
        initialMaxAnchorsForImageCircle = tutorialCircleImageRT.anchorMax;

        titleText.color = new Color(1, 1, 1, 0);
        mainText.color = new Color(1, 1, 1, 0);
        continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, 0);

        if(GlobalDefaults.Instance.isTablet)
        {
            tutorialCircleImageRT.anchoredPosition = new Vector2(-470, -200);
            initialCircleImagePos = tutorialCircleImageRT.anchoredPosition3D;
            initialVerticalLayoutGroupPos = verticalLayoutGroupRT.anchoredPosition3D;

            groupImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-360, -200);
            groupImage.GetComponent<RectTransform>().sizeDelta = new Vector2(525, 194);
        }

        if(GlobalDefaults.Instance.tutorialIsOn)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            displayTutorialScreenData();
            numberOfTutorialScreens = TutorialMainScreenData.numberOfScreens;
        }
    }

    public void turnTutorialOff()
    {
        GlobalDefaults.Instance.tutorialIsOn = false;

        if(tutorialIndexNumber == 2)
        {
            var initialWordList = gameObject.transform.Find("initialWordList");
            initialWordList.SetParent(this.storeCollectionView.transform);

            initialWordList.GetComponent<RectTransform>().anchorMin = initialMinAnchorsForStarterPack;
            initialWordList.GetComponent<RectTransform>().anchorMax = initialMaxAnchorsForStarterPack;
        }

        gameObject.SetActive(false);
    }

    public void continueTutorial()
    {
        tutorialIndexNumber += 1;
        if(tutorialIndexNumber < numberOfTutorialScreens)
        {
            displayTutorialScreenData();
        } else
        {
            turnTutorialOff();
        }
    }

    private void displayTutorialScreenData()
    {
        var tutorialScreenData = new TutorialMainScreenData(tutorialIndexNumber);

        titleText.DOFade(0, 0.5f).Play().OnComplete(() => 
            {
                titleText.text = tutorialScreenData.titleText;
                titleText.DOFade(1, 0.5f).Play();
            }   
        );

        mainText.DOFade(0, 0.5f).Play().OnComplete(() => 
            {
                mainText.text = tutorialScreenData.mainText; 
                mainText.DOFade(1, 0.5f).Play();
            }
        );

        continueText.DOFade(0, 0.5f).Play().OnComplete(() =>
            {
                continueText.text = "<Tap to continue>";

                if(tutorialIndexNumber == 5)
                {
                    continueText.text = "<Tap to exit>";
                }
                continueText.DOFade(1, 0.5f).Play();
            }
        );

        currentCanvas = GameObject.Find(tutorialScreenData.referenceCanvas);
        hideBackGroundObjects(tutorialScreenData.referenceCanvas);
    }

    private void hideBackGroundObjects(string canvasName)
    {
        if(canvasName == "StoreCanvas")
        {   
            if (tutorialIndexNumber == 1)
            {
                groupImage.GetComponent<Image>().DOFade(1, 1);
            } else 
            {
                groupImage.GetComponent<Image>().DOFade(0, 1f);
            }   

            if (tutorialIndexNumber == 2)
            {
                var initialWordList = GameObject.Find("initialWordList");
                initialWordList.transform.SetParent(this.gameObject.transform);
                initialWordList.transform.SetSiblingIndex(2);

                initialMinAnchorsForStarterPack = initialWordList.GetComponent<RectTransform>().anchorMin;
                initialMaxAnchorsForStarterPack = initialWordList.GetComponent<RectTransform>().anchorMax;

                initialWordList.GetComponent<RectTransform>().anchorMin = new Vector2(0.065f, 0.5f);
                initialWordList.GetComponent<RectTransform>().anchorMax = new Vector2(0.065f, 0.5f);     
            }
        }

        if(canvasName == "MainBoardCanvas")
        {
            backgroundCanvas.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            gameObject.GetComponent<Image>().enabled = false;

            if(tutorialIndexNumber == 3)
            {
                var gameCreationScript = gameCreation.GetComponent<GameCreationScript>();
                gameCreationScript.generateGameState();

                var initialWordList = gameObject.transform.Find("initialWordList");
                initialWordList.SetParent(this.storeCollectionView.transform);

                initialWordList.GetComponent<RectTransform>().anchorMin = initialMinAnchorsForStarterPack;
                initialWordList.GetComponent<RectTransform>().anchorMax = initialMaxAnchorsForStarterPack;
            }

            if(tutorialIndexNumber == 4)
            {
                tutorialCircleImageRT.DOAnchorMin(new Vector2(0.5f, 0.5f), 0.7f, false);
                tutorialCircleImageRT.DOAnchorMax(new Vector2(0.5f, 0.5f), 0.7f, false);
                tutorialCircleImageRT.DOSizeDelta(new Vector2(5000, 5000), 0.7f, false);

                tutorialCircleImageRT.DOAnchorPos(new Vector2(0, 0), 0.7f, false);
                verticalLayoutGroupRT.DOAnchorPos(new Vector2(0, 0), 0.7f, false);

            } else 
            {

                tutorialCircleImageRT.DOAnchorMin(initialMinAnchorsForImageCircle, 0.7f, false);
                tutorialCircleImageRT.DOAnchorMax(initialMaxAnchorsForImageCircle, 0.7f, false);
                tutorialCircleImageRT.DOSizeDelta(initialCircleImageSize, 0.7f, false);

                tutorialCircleImageRT.DOAnchorPos(initialCircleImagePos, 0.7f, false);
                verticalLayoutGroupRT.DOAnchorPos(initialVerticalLayoutGroupPos, 0.7f, false);
            }

            if(tutorialIndexNumber == 5)
            {
                exitTutorialButton.SetActive(false);   
            }
        }
    }
}
