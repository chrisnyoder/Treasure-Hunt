using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Network network;

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
        network.StopServer();
        SceneManager.LoadScene("IntroScene");
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