using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLanguageHandler : MonoBehaviour
{
    public void selectLanguage(string language)
    {
        PlayerPrefs.SetString("language", language);
        LocalizationManager.instance.setLanguage();
        reloadScene();
    }

    private void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
