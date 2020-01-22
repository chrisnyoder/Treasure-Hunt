using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusic : MonoBehaviour
{
    public Sprite musicOn;
    public Sprite musicOff;
    Image musicImage;

    private void Start() 
    {
        musicImage = gameObject.GetComponent<Image>();
        selectCorrectMusicIcon();
    }

    public void toggleMusic()
    {

        GlobalAudioScript.Instance.toggleMusic();
        
        selectCorrectMusicIcon();
    }

    private void selectCorrectMusicIcon()
    {
        if (GlobalAudioScript.Instance.backgroundMusic.enabled)
        {
            musicImage.sprite = musicOn;
        }
        else
        {
            musicImage.sprite = musicOff;
        }
    }
}
