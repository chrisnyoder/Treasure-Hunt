using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Team
{
    RedTeam, 
    BlueTeam
}

public class TutorialHiddenBoardScript : MonoBehaviour
{
    private int tutorialIndexNumber = 0; 
    private int totalNumberOfTutorialScreens; 
    private Team team;
    private Tween moveBlueButton; 

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

    public GameObject blueButton; 
    public GameObject redButton;
    public GameObject neutralButton;

    void Start()
    {
        var tutorialScreenData = new HiddenBoardTutorialData(tutorialIndexNumber);
        mainText.text = tutorialScreenData.mainText;
        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0f);
        continueButton.SetActive(false);

        blueButton = GameObject.Find("BlueButton");
        redButton = GameObject.Find("RedButton");
        neutralButton = GameObject.Find("NeutralButton");
    }

    public void beginTutorial(Team team)
    {
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
        var texts = scrollPanel.GetComponentsInChildren<Text>();

        backgroundCanvas.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        backToMainMenuButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        blueButton.GetComponent<Button>().enabled = true;
        redButton.GetComponent<Button>().enabled = true;
        neutralButton.GetComponent<Button>().enabled = true;

        moveBlueButton.Kill(true);

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

        foreach (Text text in texts)
        {
            text.DOColor(new Color32(0, 0, 0, 255), 0.5f).Play();
        }

        gameObject.SetActive(false);
    }

    public void continueTutorial()
    {
        tutorialIndexNumber += 1;
        if (tutorialIndexNumber < totalNumberOfTutorialScreens && gameObject.activeSelf == true)
        {
            displayTutorialScreenData();
        } else 
        {
            turnTutorialOff();
        }
    }

    public void displayTutorialScreenData()
    {
        mainText.DOFade(0, 0.7f).Play().OnComplete(() =>
            {            
                var tutorialScreenData = new HiddenBoardTutorialData(tutorialIndexNumber);
                mainText.text = tutorialScreenData.mainText;
                mainText.DOFade(1, 0.7f).Play();
            }
        );

        shadeImages();
        if(tutorialIndexNumber == 0)
        {
            placeCircleImageOnTheSide();
        }

        if(tutorialIndexNumber == 1)
        {
            titleText.GetComponent<Text>().DOFade(0, 0.5f).Play().OnComplete(() => 
                { 
                    Destroy(titleText); 
                }
            );

            blueButton.GetComponent<Image>().color = new Color32 (255, 255, 255, 255);
            blueButton.GetComponent<Button>().enabled = true;
            moveBlueButton = blueButton.GetComponent<RectTransform>().DOAnchorPosY(blueButton.GetComponent<RectTransform>().anchoredPosition.y - 50, 0.6f, false);
            moveBlueButton.SetEase(Ease.Linear);
            moveBlueButton.SetLoops(-1, LoopType.Yoyo);

            continueButton.SetActive(false);

            pressToContinueText.GetComponent<Text>().DOFade(0, 0.7f).Play();
        }

        if(tutorialIndexNumber == 2)
        {
            moveBlueButton.Kill(true);
            
            blueButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            
            pressToContinueText.GetComponent<Text>().DOFade(1, 0.7f).Play();

            highlightText();
            placeCircleImageOnBottom();
        }

        if(tutorialIndexNumber == 3)
        {
            placeCircleImageOnTheSide();
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
            placeCircleImageOnTheSide();
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
            placeCircleImageOnTheSide();
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
            skipTutorialButton.SetActive(false);
            pressToContinueText.GetComponent<LayoutElement>().minHeight = 200; 
            pressToContinueText.GetComponent<Text>().text = "press anywhere to exit";
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

    private void placeCircleImageOnTheSide()
    {
        var tween = circleImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-560, 345), 1f, false);
        var tween2 = verticalLayoutGroup.GetComponent<RectTransform>().DOAnchorPos(new Vector2(400, -50), 1f, false);
        var tween3 = verticalLayoutGroup.GetComponent<RectTransform>().DOSizeDelta(new Vector2(620, 780), 1f, false);

        tween.Play();
        tween2.Play();
        tween3.Play();

        tween.OnComplete(() =>
        {
            if(tutorialIndexNumber != 1)
                continueButton.SetActive(true);
        });
    }

    private void placeCircleImageOnTop()
    {
        var tween = circleImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 1475), 1f, false);
        var tween2 = verticalLayoutGroup.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -590), 1f, false);
        var tween3 = verticalLayoutGroup.GetComponent<RectTransform>().DOSizeDelta(new Vector2(950, 270), 1f, false);

        if(GlobalDefaults.Instance.isTablet)
        {
            tween = circleImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 1280), 1f, false);
            tween2 = verticalLayoutGroup.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -650), 1f, false);
            tween3 = verticalLayoutGroup.GetComponent<RectTransform>().DOSizeDelta(new Vector2(720, 270), 1f, false);   
        }

        tween.Play();
        tween2.Play();
        tween3.Play();

        tween.OnComplete(() =>
        {
            continueButton.SetActive(true);
        });
    }

    private void placeCircleImageOnBottom()
    {
        var tween = circleImage.GetComponent<RectTransform>().DOAnchorPos(new Vector2(140, -1100), 1f, false);
        var tween2 = verticalLayoutGroup.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-30, 390), 1f, false);
        var tween3 = verticalLayoutGroup.GetComponent<RectTransform>().DOSizeDelta(new Vector2(870, 520), 1f, false);

        if(GlobalDefaults.Instance.isTablet)
        {            
            tween2 = verticalLayoutGroup.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-30, 540), 1f, false);
            tween3 = verticalLayoutGroup.GetComponent<RectTransform>().DOSizeDelta(new Vector2(870, 270), 1f, false);
        }

        tween.Play();
        tween2.Play();
        tween3.Play();

        tween.OnComplete(() => 
        {
            continueButton.SetActive(true);
        });
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