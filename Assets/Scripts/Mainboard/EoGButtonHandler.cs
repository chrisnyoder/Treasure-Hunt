using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class EoGButtonHandler : MonoBehaviour
{

    public MainBoardNetworkingClient mainBoardNetworkingClient; 

    public void restartGame()
    {
        moveEoGScreen();

        var StoreCanvasObject = GameObject.Find("StoreCanvas");
        var StoreCanvasAnimator = StoreCanvasObject.GetComponent<Animator>();
        StoreCanvasAnimator.Play("StoreAnimationReverse");

        var CodeDisplayBackgroundObject = GameObject.Find("CodeDisplayBackground");

        var CodeDisplayBackroundRT = CodeDisplayBackgroundObject.GetComponent<RectTransform>();
        CodeDisplayBackroundRT.DOAnchorPosY(0, 1.5f, false);
        CodeDisplayBackgroundObject.GetComponent<Image>().DOFade(0, 0.1f);
    
        var MainBoardCanvasObject = GameObject.Find("MainBoardCanvas");

        var mainBoardrt = MainBoardCanvasObject.GetComponent<RectTransform>();
        mainBoardrt.DOAnchorPosY(-1500, 0.7f, false);

        mainBoardNetworkingClient.initialGameState = null;
        mainBoardNetworkingClient.gameStateSent = false;
    }
    
    public void showBoard()
    {
        moveEoGScreen();    
    }

    private void moveEoGScreen()
    {
        var EoGCanvasObject = GameObject.Find("ResultsCanvas");
        EoGCanvasObject.GetComponent<Image>().DOFade(0, 0.1f).Play();
        if (Screen.width < Screen.height)
        {
            EoGCanvasObject.GetComponent<RectTransform>().DOAnchorPosY(3000, 0.7f).Play();
        }
        else
        {
            EoGCanvasObject.GetComponent<RectTransform>().DOAnchorPosY(1500, 0.7f).Play();
        }
    }
}


