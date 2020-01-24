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

        GlobalAudioScript.Instance.playSfxSound("Locked_Down_06");
        SceneManager.LoadScene("MainBoardContainer");
    }

    public void goToJoinGameInterstitial()
    {
        SceneManager.LoadScene("JoinGameInterstitial");
        GlobalAudioScript.Instance.playSfxSound("Locked_Down_06");
    }

    public void GoToMainBoardAsCrew()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();
        GlobalAudioScript.Instance.playSfxSound("Locked_Down_06");

        hiddenBoardTransitionImage.DOFade(1, 0.2f).Play().OnComplete(() =>
        {
            SceneManager.LoadSceneAsync("JoinMainBoardContainer");
        });
    }

    public void GoToHiddenBoard()
    {
        GlobalAudioScript.Instance.ambientSounds.Stop();
        GlobalAudioScript.Instance.playSfxSound("Locked_Down_06");

        hiddenBoardTransitionImage.DOFade(1, 0.2f).Play().OnComplete(() => 
        {
            SceneManager.LoadScene("HiddenBoardScene");
        });
    }

    public void GoToIntroScreen()
    {
        closeNetworkingClient();
        SceneManager.LoadScene("IntroScene");
        if(PlayerPrefs.GetString("backgroundMusicOn") == "true")
        {
            var audio = GameObject.Find("GlobalAudioSource").GetComponent<GlobalAudioScript>();
            audio.backgroundMusic.enabled = true;
        }
    }

    public void bringUpExitPopUpOnMainboard()
    {
        GlobalAudioScript.Instance.playSfxSound("Locked_Down_06");

        var exitPopUpCanvasRT = backToMainMenuCanvas.GetComponent<RectTransform>();
        exitPopUpCanvasRT.anchoredPosition = new Vector2(0, 1500);
        exitPopUpCanvasRT.DOAnchorPosY(0, 0.7f).Play().OnComplete(() => {
            exitPopUpCanvasRT.GetComponent<Image>().DOFade(0.627f, 0.3f).Play();
        }
        );
    }

    public void DismissExitMenuPopUpOnMainboard()
    {
        GlobalAudioScript.Instance.playSfxSound("closeDrawer");

        var exitPopUpCanvasRT = backToMainMenuCanvas.GetComponent<RectTransform>();

        exitPopUpCanvasRT.DOAnchorPosY(1500, 0.7f).Play();
        exitPopUpCanvasRT.GetComponent<Image>().DOFade(0f, 0.1f).Play();
    }

    public void bringUpExitPopUpOnHiddenboard()
    {
        GlobalAudioScript.Instance.playSfxSound("openDrawer");

        var exitPopUpCanvasRT = backToMainMenuCanvas.GetComponent<RectTransform>();
        exitPopUpCanvasRT.anchoredPosition = new Vector2(0, 3000);
        exitPopUpCanvasRT.DOAnchorPosY(0, 0.7f).Play().OnComplete(() =>
        {
            exitPopUpCanvasRT.GetComponent<Image>().DOFade(0.627f, 0.3f).Play();
        }
        );
    }

    public void DismissExitMenuPopUpOnHiddenboard()
    {
        GlobalAudioScript.Instance.playSfxSound("closeDrawer");

        var exitPopUpCanvasRT = backToMainMenuCanvas.GetComponent<RectTransform>();

        exitPopUpCanvasRT.DOAnchorPosY(3000, 0.7f).Play();
        exitPopUpCanvasRT.GetComponent<Image>().DOFade(0f, 0.1f).Play();
    }

    public void bringUpInfoPopUp()
    {

        GlobalAudioScript.Instance.playSfxSound("Locked_Down_06");
        var infoRT = infoPopUp.GetComponent<RectTransform>();
        infoRT.DOAnchorPosY(0, 0.5f, false).Play().OnComplete(() => {
            infoRT.GetComponent<Image>().DOFade(0.627f, 0.3f);
        });
    }

    public void closeInfoPopUp()
    {
        GlobalAudioScript.Instance.playSfxSound("Slam_Metal_03");

        var infoRT = infoPopUp.GetComponent<RectTransform>();
        infoRT.DOAnchorPosY(1500, 0.7f, false).Play();
        infoRT.GetComponent<Image>().DOFade(0, .001f);
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