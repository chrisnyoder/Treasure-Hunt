using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Network network;
    public Canvas backToMainMenuCanvas;

    public void GoToMainBoard()
    {
        SceneManager.LoadScene("MainBoardContainer");
    }

    public void GoToHiddenBoard()
    {
        SceneManager.LoadScene("HiddenBoardScene");
    }

    public void GoToIntroScreenFromMainBoard()
    {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();

        if(network != null)
        {
            network.StopServer();
        }

        SceneManager.LoadScene("IntroScene");
    }

    public void bringUpExitPopUpOnMainboard()
    {
        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuAnimation");
    }

    public void DismissExitMenuPopUpOnMainboard()
    {

        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuAnimationReverse");
    }

    public void bringUpExitPopUpOnHiddenboard()
    {
        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuFromHiddenboardAnimation");
    }

    public void DismissExitMenuPopUpOnHiddenboard()
    {
        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuFromHiddenboardAnimationReverse");
    }

    public void GoToIntroScreenFromHiddenBoard()
    {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();
        network.StopClient();
        SceneManager.LoadScene("IntroScene");
    }

    public void GoToStoreScreen()
    {
        SceneManager.LoadScene("Store");
    }
}