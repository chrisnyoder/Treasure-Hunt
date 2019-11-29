using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    private Scene scene;

    public Canvas backToMainMenuCanvas;
    public Canvas infoPopUp;
    public Canvas joinGamePopUpCanvas;

    private Vector2 initialJoinGamePopUpPos;

    public MainBoardNetworkingClient mainBoardNetworkingClient; 
    public HiddenBoardNetworkingClient hiddenBoardNetworkingClient;
    public CrewNetworkingClient crewNetworkingClient;

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

    public void bringUpJoinGamePopUp()
    {
        initialJoinGamePopUpPos = joinGamePopUpCanvas.GetComponent<RectTransform>().anchoredPosition;
        joinGamePopUpCanvas.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f, false).SetEase(Ease.Linear).Play();
    }

    public void GoToMainBoardAsCrew()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();

        GlobalAudioScript.Instance.playSfxSound("openMenu");
        SceneManager.LoadScene("JoinMainBoardContainer");
    }

    public void GoToHiddenBoard()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();

        GlobalAudioScript.Instance.playSfxSound("openMenu");
        SceneManager.LoadScene("HiddenBoardScene");
    }

    public void GoToIntroScreenFromMainBoard()
    {
        GlobalAudioScript.Instance.playSfxSound("openMenu");
        mainBoardNetworkingClient.Close();
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
        GlobalAudioScript.Instance.playSfxSound("openMenu");
        hiddenBoardNetworkingClient.Close();
        SceneManager.LoadScene("IntroScene");
    }

    public void GoToIntroScreenFromCrewBoard()
    {
        GlobalAudioScript.Instance.playSfxSound("openMenu");
        crewNetworkingClient.Close();
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

    public void closeJoinGamePopUp()
    {
        joinGamePopUpCanvas.GetComponent<RectTransform>().DOAnchorPosY(initialJoinGamePopUpPos.y, 0.5f, false).SetEase(Ease.Linear).Play();
    }
}