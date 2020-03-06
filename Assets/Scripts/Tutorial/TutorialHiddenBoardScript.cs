using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public enum Team
{
    RedTeam,
    BlueTeam
}

public class TutorialHiddenBoardScript : MonoBehaviour
{
    private int tutorialIndexNumber = 0;
    private int totalNumberOfTutorialScreens;
    private Team _team;
    private Tween moveButton;
    private HiddenBoardTutorialData tutorialScreenData;

    public GameObject titleText;
    public Text mainText;
    public GameObject pressToContinueText;

    public GameObject backgroundCanvas;
    public GameObject backToMainMenuButton;
    public GameObject scrollPanel;
    public GameObject continueButton;
    public GameObject skipTutorialButton;
    public GameObject circleImage;
    public GameObject verticalLayoutGroup;
    public GameObject timerObject; 

    private Vector2 circleInitialMinAnchor; 
    private Vector2 circleInitialMaxAnchor; 

    public GameObject blueButton;
    public GameObject redButton;
    public GameObject neutralButton;

    private GameObject tabToMove;

    void Start()
    {
        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0f);
        continueButton.SetActive(false);

        blueButton = GameObject.Find("BlueButton");
        redButton = GameObject.Find("RedButton");
        neutralButton = GameObject.Find("NeutralButton");

        circleInitialMinAnchor = new Vector2(0.5f, 0.5f);
        circleInitialMaxAnchor = new Vector2(0.5f, 0.5f); ;
    }

    public void beginTutorial(Team team)
    {
        GlobalDefaults.Instance.tutorialIsOn = true;
        this._team = team;

        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        displayTutorialScreenData();
        totalNumberOfTutorialScreens = HiddenBoardTutorialData.numberOfScreens;

        redButton.GetComponent<Button>().enabled = false;
        blueButton.GetComponent<Button>().enabled = false;
        neutralButton.GetComponent<Button>().enabled = false;
    }

    public void turnTutorialOff()
    {
        GlobalDefaults.Instance.tutorialIsOn = false;

        var images = scrollPanel.GetComponentsInChildren<Image>();
        var timerImages = timerObject.GetComponentsInChildren<Image>();
        var texts = scrollPanel.GetComponentsInChildren<Text>();

        backgroundCanvas.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        backToMainMenuButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        blueButton.GetComponent<Button>().enabled = true;
        redButton.GetComponent<Button>().enabled = true;
        neutralButton.GetComponent<Button>().enabled = true;

        moveButton.Kill(true);

        foreach (Image image in images)
        {
            if (image.gameObject.name == "Panel")
            {
                image.color = new Color32(255, 255, 255, 0);
            }
            else
            {
                image.DOColor(new Color32(255, 255, 255, 255), 0.5f).Play();
            }
        }

        foreach (Image image in timerImages)
        {
            image.DOColor(new Color32(255, 255, 255, 255), 0.5f).Play();
        }

        foreach (Text text in texts)
        {
            if (text.gameObject.name == "BlueTabText")
            {
                text.GetComponent<Text>().DOColor(new Color32(217, 217, 217, 255), 0.5f).Play();
            } else if(text.gameObject.name == "RedTabText")
            {
                text.GetComponent<Text>().DOColor(new Color32(255, 194, 194, 255), 0.5f).Play();
            } else if(text.gameObject.name == "NeutralTabText")
            {
                text.GetComponent<Text>().DOColor(new Color32(89, 89, 89, 255), 0.5f).Play();
            }
            else 
            {
                text.DOColor(new Color32(0, 0, 0, 255), 0.5f).Play();
            }
        }
        gameObject.SetActive(false);
    }

    public void continueTutorial()
    {
        if(GlobalDefaults.Instance.tutorialIsOn)
        {
            tutorialIndexNumber += 1;
            if (tutorialIndexNumber < totalNumberOfTutorialScreens && gameObject.activeSelf == true)
            {
                displayTutorialScreenData();
            }
            else
            {
                turnTutorialOff();
            }
        }
    }

    public void displayTutorialScreenData()
    {

        shadeImages();
        if (tutorialIndexNumber == 0)
        {
            placeCircleImageOnTheSide();
        }

        if (tutorialIndexNumber == 1)
        {
            placeCircleImageOnTheSide();

            titleText.GetComponent<Text>().DOFade(0, 0.5f).Play().OnComplete(() =>
                {
                    Destroy(titleText);
                }
            );

            tabToMove = blueButton;

            if (_team == Team.RedTeam)
                tabToMove = redButton;

            tabToMove.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            tabToMove.GetComponent<Button>().enabled = true;
            tabToMove.GetComponentInChildren<Text>().color = new Color32(255, 239, 210, 255);
            moveButton = tabToMove.GetComponent<RectTransform>().DOAnchorPosY(tabToMove.GetComponent<RectTransform>().anchoredPosition.y - 40, 0.6f, false);
            moveButton.SetEase(Ease.Linear);
            moveButton.SetLoops(-1, LoopType.Yoyo);

            continueButton.SetActive(false);

            pressToContinueText.GetComponent<Text>().DOFade(0, 0.7f).Play();
        }

        if (tutorialIndexNumber == 2)
        {
            placeCircleImageOnBottom();

            moveButton.Kill(true);

            tabToMove.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            pressToContinueText.GetComponent<Text>().DOFade(1, 0.7f).Play();

            highlightText();
        }

        if (tutorialIndexNumber == 3)
        {

                placeCircleImageOnTheSide();
        }

        if (tutorialIndexNumber == 4 || tutorialIndexNumber == 5)
        {
            var texts = scrollPanel.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if (text.text == "Saturn" || text.text == "satellite")
                {
                    text.color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    text.color = new Color32(255, 255, 255, 120);
                }
            }
            placeCircleImageOnBottom();
        }

        if (tutorialIndexNumber == 6)
        {
            placeCircleImageOnTheSide();
        }

        if (tutorialIndexNumber == 7)
        {
            highlightButtons();
            highlightText();
            placeCircleImageOnTop();
        }

        if (tutorialIndexNumber == 8)
        {

            placeCircleImageOnTheSide();

            var dangerImage = GameObject.Find("DangerImage");
            var dangerText = GameObject.Find("DangerText");

            dangerImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            dangerText.GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(650, 800);
        }

        if (tutorialIndexNumber == 9)
        {
            placeCircleImageOnTheSide();
            var dangerText = GameObject.Find("DangerText");
            dangerText.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
            skipTutorialButton.SetActive(false);
            pressToContinueText.GetComponent<LayoutElement>().minHeight = 200;
            pressToContinueText.GetComponent<Text>().text = LocalizationManager.instance.GetLocalizedText("tap_to_exit");
        }
    }

    private void resetCircleImageAnchors()
    {
        circleImage.GetComponent<RectTransform>().anchorMin = circleInitialMinAnchor;
        circleImage.GetComponent<RectTransform>().anchorMax = circleInitialMaxAnchor;
    }

    private void fadeTextAndCircleIn()
    {
        tutorialScreenData = new HiddenBoardTutorialData(tutorialIndexNumber, _team);
        mainText.text = tutorialScreenData.mainText;
        mainText.DOFade(1, 0.5f).Play();
        circleImage.GetComponent<Image>().DOFade(1, 0.5f);
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

    private void placeCircleImageOnTheSide()
    {
        var tween = circleImage.GetComponent<Image>().DOFade(0, 0.5f);
        tween.Play();

        mainText.DOFade(0, 0.5f).Play();

        tween.OnComplete(() =>
        {
            circleImage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            circleImage.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);

            circleImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 345);
            verticalLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, -50);
            verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(620, 780);

            fadeTextAndCircleIn();
            if (tutorialIndexNumber != 1)
                continueButton.SetActive(true);
        });
    }

    private void placeCircleImageOnTop()
    {

        var tween = circleImage.GetComponent<Image>().DOFade(0, 0.5f);

        mainText.DOFade(0, 0.5f).Play();
        tween.Play();


        tween.Play();

        tween.OnComplete(() =>
        {
            circleImage.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            circleImage.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);

            circleImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 360);
            verticalLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -600);
            verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(950, 350);

            fadeTextAndCircleIn();
            continueButton.SetActive(true);
        });
    }

    private void placeCircleImageOnBottom()
    {
        var tween = circleImage.GetComponent<Image>().DOFade(0, 0.5f);

        mainText.DOFade(0, 0.5f).Play();
        tween.Play();

        tween.OnComplete(() =>
        {
            circleImage.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0f);
            circleImage.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0f);

            circleImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(140, 120);
            verticalLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(-30, 300);
            verticalLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(870, 700);

            fadeTextAndCircleIn();
            continueButton.SetActive(true);
        });
    }

    private void shadeImages()
    {
        backgroundCanvas.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
        backToMainMenuButton.GetComponent<Image>().color = new Color32(90, 90, 90, 255);

        var images = scrollPanel.GetComponentsInChildren<Image>();
        var texts = scrollPanel.GetComponentsInChildren<Text>();
        var timerImages = timerObject.GetComponentsInChildren<Image>();

        foreach(Image image in timerImages)
        {
            image.color = new Color32(90, 90, 90, 255);

        }

        foreach (Image image in images)
        {
            if (image.gameObject.name == "Panel")
            {
                image.color = new Color32(255, 255, 255, 0);
            } 
            else
            {
                image.color = new Color32(90, 90, 90, 255);
            }
        }

        foreach (Text text in texts)
        {
            text.color = new Color32(90, 90, 90, 255);
        }
    }
}