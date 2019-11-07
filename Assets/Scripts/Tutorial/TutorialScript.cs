using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{        
    private int tutorialIndexNumber = 0; 
    private int numberOfTutorialScreens;

    private Vector2 initialCircleImageSize; 
    private Vector3 initialCircleImagePos; 
    private Vector3 initialVerticalLayoutGroupPos; 

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
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3000, 0);
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
            backgroundCanvas.GetComponent<Image>().color = new Color32(30, 30, 30, 255);

            var images = currentCanvas.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.color = new Color32(30, 30, 30, 255);
            } 

            var texts = currentCanvas.GetComponentsInChildren<Text>();
            foreach(Text text in texts)
            {
                text.color = new Color32(30, 30, 30, 255);
            }

            if (tutorialIndexNumber == 1)
            {
                groupImage.SetActive(true);
            } else 
            {
                groupImage.SetActive(false);
            }   

            if (tutorialIndexNumber == 2)
            {
                var starterPack = GameObject.Find("initialWordList");
                var starterPackImages = starterPack.GetComponentsInChildren<Image>();

                foreach(Image image in starterPackImages)
                {
                    image.color = new Color(255, 255, 255, 255);
                }             
            }
        }

        if(canvasName == "MainBoardCanvas")
        {
            backgroundCanvas.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            if(tutorialIndexNumber == 3)
            {
                var gameCreationScript = gameCreation.GetComponent<GameCreationScript>();
                gameCreationScript.generateGameState();
            }

            if(tutorialIndexNumber == 4)
            {
                tutorialCircleImageRT.sizeDelta = new Vector2(10000, 10000);
                tutorialCircleImageRT.anchoredPosition3D = new Vector3(0, 0, 0);
                verticalLayoutGroupRT.anchoredPosition3D = new Vector3(0, 0, 0);
            } else 
            {
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
