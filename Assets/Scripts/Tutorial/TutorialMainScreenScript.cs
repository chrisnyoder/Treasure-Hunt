using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        groupImage.SetActive(false);

        tutorialCircleImageRT = tutorialCircleImage.GetComponent<RectTransform>();
        verticalLayoutGroupRT = verticalLayoutGroup.GetComponent<RectTransform>();    

        initialCircleImageSize = tutorialCircleImageRT.sizeDelta;
        initialCircleImagePos = tutorialCircleImageRT.anchoredPosition3D;
        initialVerticalLayoutGroupPos = verticalLayoutGroupRT.anchoredPosition3D;

        initialMinAnchorsForImageCircle = tutorialCircleImageRT.anchorMin;
        initialMaxAnchorsForImageCircle = tutorialCircleImageRT.anchorMax;

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
        }
    }

    private void displayTutorialScreenData()
    {
        var tutorialScreenData = new TutorialMainScreenData(tutorialIndexNumber);
        
        titleText.text = tutorialScreenData.titleText;
        mainText.text = tutorialScreenData.mainText;

        currentCanvas = GameObject.Find(tutorialScreenData.referenceCanvas);

        hideBackGroundObjects(tutorialScreenData.referenceCanvas);
    }

    private void hideBackGroundObjects(string canvasName)
    {
        if(canvasName == "StoreCanvas")
        {   
            if (tutorialIndexNumber == 1)
            {
                groupImage.SetActive(true);
            } else 
            {
                groupImage.SetActive(false);
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
                tutorialCircleImageRT.anchorMin = new Vector2(0.5f, 0.5f);
                tutorialCircleImageRT.anchorMax = new Vector2(0.5f, 0.5f);
                tutorialCircleImageRT.sizeDelta = new Vector2(10000, 10000);
                tutorialCircleImageRT.anchoredPosition3D = new Vector3(0, 0, 0);
                verticalLayoutGroupRT.anchoredPosition3D = new Vector3(0, 0, 0);

            } else 
            {
                tutorialCircleImageRT.anchorMin = initialMinAnchorsForImageCircle;
                tutorialCircleImageRT.anchorMax = initialMaxAnchorsForImageCircle;
                tutorialCircleImageRT.sizeDelta = initialCircleImageSize;
                tutorialCircleImageRT.anchoredPosition3D = initialCircleImagePos;
                verticalLayoutGroupRT.anchoredPosition3D = initialVerticalLayoutGroupPos;
            }

            if(tutorialIndexNumber == 5)
            {
                continueText.enabled = false;
            }
        }
    }
}
