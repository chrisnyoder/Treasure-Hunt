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

    public void bringUpExitPopUp()
    {
        var exitPopUpCanvasRT = backToMainMenuCanvas.GetComponent<RectTransform>();

        exitPopUpCanvasRT.localPosition = new Vector3(0, 0, 0);
    }

    public void DismissExitMenuPopUP()
    {
        var backToMainMenuCanvasRT = backToMainMenuCanvas.GetComponent<RectTransform>();

        backToMainMenuCanvasRT.localPosition = new Vector3(-3000, 0, 0);
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