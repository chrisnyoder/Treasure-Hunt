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
    public GameObject HiddenBoardCanvas;

    void Start()
    {
        if (GlobalDefaults.Instance.tutorialIsOn)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            displayTutorialScreenData();
            totalNumberOfTutorialScreens = HiddenBoardTutorialData.numberOfScreens;
        }
    }

    public void turnTutorialOff()
    {
        GlobalDefaults.Instance.tutorialIsOn = false;
        gameObject.SetActive(false);
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
    }
}