using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Network network;

    private Scene scene;

    public Canvas backToMainMenuCanvas;
    public Canvas infoPopUp;

    private void Start() 
    {
        scene = SceneManager.GetActiveScene();    
    }

    public void GoToMainBoard()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();

        GlobalAudioScript.Instance.playSfxSound("openMenu");
        SceneManager.LoadScene("MainBoardContainer");
    }

    public void GoToHiddenBoard()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();

        GlobalAudioScript.Instance.playSfxSound("openMenu");
        SceneManager.LoadScene("HiddenBoardScene");
    }

    public void GoToIntroScreenFromMainBoard()
    {

        if(GameObject.Find("NetworkManager") != null)
        {
            network = GameObject.Find("NetworkManager").GetComponent<Network>();
        } 

        if(Network.networking != null)
        {
            network.StopServer();
        }

        GlobalAudioScript.Instance.playSfxSound("openMenu");
        SceneManager.LoadScene("IntroScene");
    }

    public void bringUpExitPopUpOnMainboard()
    {
        GlobalAudioScript.Instance.playSfxSound("openDrawer");

        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuAnimation");
    }

    public void DismissExitMenuPopUpOnMainboard()
    {
        GlobalAudioScript.Instance.playSfxSound("closeDrawer");

        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuAnimationReverse");
    }

    public void bringUpExitPopUpOnHiddenboard()
    {
        GlobalAudioScript.Instance.playSfxSound("openDrawer");

        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuFromHiddenboardAnimation");
    }

    public void DismissExitMenuPopUpOnHiddenboard()
    {
        GlobalAudioScript.Instance.playSfxSound("closeDrawer");

        var exitPopUpCanvasAnimator = backToMainMenuCanvas.GetComponent<Animator>();
        exitPopUpCanvasAnimator.Play("BackToMainMenuFromHiddenboardAnimationReverse");
    }

    public void GoToIntroScreenFromHiddenBoard()
    {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();
        network.StopClient();

        GlobalAudioScript.Instance.playSfxSound("openMenu");
        SceneManager.LoadScene("IntroScene");
    }

    public void bringUpInfoPopUp()
    {
        GlobalAudioScript.Instance.playSfxSound("openDrawer");

        var animator = infoPopUp.GetComponent<Animator>();
        animator.Play("StoreInfoPopUpAnimation");
    }

    public void closeInfoPopUp()
    {
        GlobalAudioScript.Instance.playSfxSound("closeDrawer");

        var animator = infoPopUp.GetComponent<Animator>();
        animator.Play("StoreInfoPopUpAnimationReverse");
    }

}