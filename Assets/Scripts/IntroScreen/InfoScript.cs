using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InfoScript : MonoBehaviour 
{
    public Image musicImage;

    public Sprite musicOn;
    public Sprite musicOff;

    private AudioSource backgroundMusic; 

    private void Start() 
    {
        backgroundMusic = GlobalAudioScript.Instance.GetComponents<AudioSource>()[2];
        selectCorrectMusicIcon();
    }

    private void selectCorrectMusicIcon()
    {
        if (backgroundMusic.enabled)
        {
                musicImage.sprite = musicOn;
        } else 
        {
            musicImage.sprite = musicOff;
        }
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

