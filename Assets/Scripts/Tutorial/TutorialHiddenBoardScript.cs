using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHiddenBoardScript : MonoBehaviour
{
    private int tutorialIndexNumber = 0; 
    private int totalNumberOfTutorialScreens; 

    public GameObject titleText; 
    public Text mainText;

    public GameObject backgroundCanvas; 
    public GameObject backToMainMenuButton; 
    public GameObject scrollPanel;
    public GameObject continueButton; 
    public GameObject circleImage; 
    public GameObject verticalLayoutGroup; 

    public GameObject blueButton; 
    public GameObject redButton;
    public GameObject neutralButton;

    void Start()
    {
        if (GlobalDefaults.Instance.tutorialIsOn)
        {
            blueButton = GameObject.Find("BlueButton");
            redButton = GameObject.Find("RedButton");
            neutralButton = GameObject.Find("NeutralButton");

            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            displayTutorialScreenData();
            totalNumberOfTutorialScreens = HiddenBoardTutorialData.numberOfScreens;
        }
    }

    public void turnTutorialOff()
    {
        GlobalDefaults.Instance.tutorialIsOn = false;
        gameObject.SetActive(false);

        var images = scrollPanel.GetComponentsInChildren<Image>();
        var texts = scrollPanel.GetComponentsInChildren<Text>();

        backgroundCanvas.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        backToMainMenuButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        blueButton.GetComponent<Button>().enabled = true;
        redButton.GetComponent<Button>().enabled = true;
        neutralButton.GetComponent<Button>().enabled = true;

        foreach (Image image in images)
        {
            if (image.gameObject.name == "Panel")
            {
                image.color = new Color32(255, 255, 255, 0);
            }
            else
            {
                image.color = new Color32(255, 255, 255, 255);
            }
        }

        foreach (Text text in texts)
        {
            text.color = new Color32(0, 0, 0, 255);
        }
    }

    public void continueTutorial()
    {
        Destroy(titleText);
        tutorialIndexNumber += 1;
        if (tutorialIndexNumber < totalNumberOfTutorialScreens)
        {
            displayTutorialScreenData();
        }
    }

    void displayTutorialScreenData()
    {
        var tutorialScreenData = new HiddenBoardTutorialData(tutorialIndexNumber);
        mainText.text = tutorialScreenData.mainText;

        circleImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-560, 345);
        verticalLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, -50);

        shadeImages();

        if(tutorialIndexNumber == 1)
        {
            blueButton.GetComponent<Image>().color = new Color32 (255, 255, 255, 255);
            continueButton.SetActive(false);
            blueButton.GetComponent<Button>().enabled = true;
        }

        if(tutorialIndexNumber == 2)
        {
            highlightText();
            placeCircleImageOnBottom();
        }
        
        if(tutorialIndexNumber == 4 || tutorialIndexNumber == 5)
        {
            var texts = scrollPanel.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if(text.text == "Saturn" || text.text == "satellite")
                {
                    text.color = new Color32(255, 255, 255, 255);
                } else 
                {
                    text.color = new Color32(255, 255, 255, 120);
                }
            }
            placeCircleImageOnBottom();
        }

        if(tutorialIndexNumber == 6)
        {
            highlightButtons();
        }

        if (tutorialIndexNumber == 7)
        {
            highlightButtons();
            highlightText();
            placeCircleImageOnTop();
        }

        if(tutorialIndexNumber == 8)
        {
            var dangerImage = GameObject.Find("DangerImage");
            var dangerText = GameObject.Find("DangerText");

            dangerImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            dangerText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(650, 800);
        }

        if (tutorialIndexNumber == 9)
        {
            var dangerText = GameObject.Find("DangerText");
            dangerText.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        }

    }

    private void highlightText()
    {
        var texts = scrollPanel.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.color = new Color32(255, 255, 255, 255);
        }
    }

    private void highlightButtons()
    {
        redButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        blueButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        neutralButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void placeCircleImageOnTop()
    {
        circleImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 1500);
        verticalLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(60, -540);
        verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 460);
        continueButton.SetActive(true);
    }

    private void placeCircleImageOnBottom()
    {
        circleImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(280, -1150);
        verticalLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 360);
        verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 650);
        continueButton.SetActive(true);
    }

    private void shadeImages()
    {
        backgroundCanvas.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
        backToMainMenuButton.GetComponent<Image>().color = new Color32(90, 90, 90, 255);

        var images = scrollPanel.GetComponentsInChildren<Image>(); 
        var texts = scrollPanel.GetComponentsInChildren<Text>();

        foreach(Image image in images)
        {
            if(image.gameObject.name == "Panel")
            {
                image.color = new Color32(255, 255, 255, 0);
            }else 
            {
                image.color = new Color32(90, 90, 90, 255);
            }
        }

        foreach(Text text in texts)
        {
            text.color = new Color32(90, 90, 90, 255);
        }
    }
}