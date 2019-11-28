using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InfoScript : MonoBehaviour 
{
    public Image musicImage;
    public Image tutorialImage;

    public Sprite musicOn;
    public Sprite musicOff;

    private AudioSource backgroundMusic; 
    private bool tutorialOn;

    private void Start() 
    {
        backgroundMusic = GlobalAudioScript.Instance.GetComponents<AudioSource>()[2];
        tutorialOn = GlobalDefaults.Instance.tutorialIsOn;
        
        selectCorrectMusicIcon();
        selectCorrectTutorialIcon();
    }

    private string MyEscapeURL(string URL)
    {
        return UnityWebRequest.EscapeURL(URL).Replace("+", "%20");
    }

    public void toggleMusic()
    {
        if(backgroundMusic.enabled)
        {
            backgroundMusic.enabled = false;
        } else 
        {
            backgroundMusic.enabled = true;
        }

        GlobalAudioScript.Instance.playSfxSound("togglePack2");
        selectCorrectMusicIcon();
    }

    private void selectCorrectMusicIcon()
    {
        if (backgroundMusic.enabled)
        {
            musicImage.sprite = musicOn;
        }
        else
        {
            musicImage.sprite = musicOff;
        }
    }

    public void toggleTutorial()
    {
        if(GlobalDefaults.Instance.tutorialIsOn)
        {
            print("tutorial is on, turning off");
            GlobalDefaults.Instance.tutorialIsOn = false;
        } else 
        {
            print("tutorial is off, turning on");
            GlobalDefaults.Instance.tutorialIsOn = true;
        }
        
        GlobalAudioScript.Instance.playSfxSound("togglePack2");
        selectCorrectTutorialIcon();
    }

    private void selectCorrectTutorialIcon()
    {
        if (GlobalDefaults.Instance.tutorialIsOn)
        {
            tutorialImage.sprite = Resources.Load<Sprite>("Images/UIElements/music_icon");
        }
        else
        {
            tutorialImage.sprite = Resources.Load<Sprite>("Images/UIElements/music");
        }
    }

    public void contactFriendlyPixel()
    {
        GlobalAudioScript.Instance.playSfxSound("togglePack2");

        string email = "contact@friendlypixel.app";
        string subject = MyEscapeURL("Subject");
        string body = MyEscapeURL("");

        print("mailto:" + email + "?subject=" + subject + "%body=" + body);

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    public void bringUpPrivacyPolicy()
    {
        GlobalAudioScript.Instance.playSfxSound("togglePack2");
        Application.OpenURL("https://friendlypixel.app/treasure_hunt_privacy_policy.html");
    }
}

