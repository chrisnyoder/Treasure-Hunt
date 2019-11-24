using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EoGButtonHandler : MonoBehaviour
{

    public void restartGame()
    {
        var EoGCanvasObject = GameObject.Find("ResultsCanvas");
        var EoGCanvasAninmator = EoGCanvasObject.GetComponent<Animator>();

        var StoreCanvasObject = GameObject.Find("StoreCanvas");
        var StoreCanvasAnimator = StoreCanvasObject.GetComponent<Animator>();

        var MainBoardCanvasObject = GameObject.Find("MainBoardCanvas");
        var MainBoardCanvasAnimator = MainBoardCanvasObject.GetComponent<Animator>();

        var CodeDisplayBackroundRT = GameObject.Find("CodeDisplayBackground").GetComponent<RectTransform>();
        CodeDisplayBackroundRT.DOAnchorPosY(0, 1.5f, false);

        var cards = EoGCanvasObject.GetComponent<EoGScript>().cards;

        foreach(GameObject card in cards)
        {
            Destroy(card);
        }
       
        EoGCanvasAninmator.Play("ResultsAnimationReverse");
        StoreCanvasAnimator.Play("StoreAnimationReverse");
        MainBoardCanvasAnimator.Play("MainboardCanvasReverseAnimation");

    }
    
    public void showBoard()
    {
        var EoGCanvasObject = GameObject.Find("ResultsCanvas");
        var EoGCanvasAninmator = EoGCanvasObject.GetComponent<Animator>();

        if(Screen.width < Screen.height) 
        {
            EoGCanvasAninmator.Play("HiddenBoardResultsReverse");
        } else
        {
            EoGCanvasAninmator.Play("ResultsAnimationReverse");
        }
            
    }

}


