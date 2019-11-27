using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoFade : MonoBehaviour
{

    public Image splashImage;
    private string loadLevel = "IntroScene";

    IEnumerator Start()
    {   
        splashImage.canvasRenderer.SetAlpha(0.0f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);

        FadeOut();
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(loadLevel);
    }


    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }


    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, 2.0f, false);
    }
}
