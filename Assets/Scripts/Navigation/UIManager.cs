using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public enum Role
{
    captain,
    crew
}

public class UIManager : MonoBehaviour
{
    private Scene scene;

    public Canvas backToMainMenuCanvas;
    public Canvas infoPopUp;
    public Image hiddenBoardTransitionImage; 

    private Vector2 initialJoinGamePopUpPos;  
    public WSNetworkingClient networkingClient;

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

    public void goToJoinGameInterstitial()
    {
        SceneManager.LoadScene("JoinGameInterstitial");
    }

    public void GoToMainBoardAsCrew()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();
        GlobalAudioScript.Instance.playSfxSound("openMenu");

        hiddenBoardTransitionImage.DOFade(1, 0.2f).Play().OnComplete(() =>
        {
            SceneManager.LoadScene("JoinMainBoardContainer");
        });
    }

    public void GoToHiddenBoard()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();
        GlobalAudioScript.Instance.playSfxSound("openMenu");

        hiddenBoardTransitionImage.DOFade(1, 0.2f).Play().OnComplete(() => 
        {
            SceneManager.LoadScene("HiddenBoardScene");
        });
    }

    public void GoToIntroScreen()
    {
        closeNetworkingClient();
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

    private void closeNetworkingClient()
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();

        if(networkingClient != null) 
        {
            networkingClient.Close();
            Destroy(networkingClient.gameObject);
        }
    }
}