using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioScript : MonoBehaviour
{
    private static GlobalAudioScript _instance;
    public static GlobalAudioScript Instance 
    {
        get
        {
            return _instance;
        }
    }

    public AudioSource soundfx;
    public AudioSource ambientSounds;
    public AudioSource backgroundMusic; 
    private AudioClip audioClip;

    private void Awake() 
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        soundfx = GetComponents<AudioSource>()[0];
        ambientSounds = GetComponents<AudioSource>()[1];
        backgroundMusic = GetComponents<AudioSource>()[2];

        playAmbientSounds("jungle_sfx");

        setMusicDefaults();
        DontDestroyOnLoad(this.gameObject);
    }

    public void playAmbientSounds(string soundName)
    {
        audioClip = (AudioClip)Resources.Load("Audio/" + soundName);
        ambientSounds.clip = audioClip;
        ambientSounds.Play();
    }

    public void playSfxSound(string soundName)
    {
        audioClip = (AudioClip) Resources.Load("Audio/" + soundName);
        soundfx.clip = audioClip;
        soundfx.Play();
    }

    public void toggleMusic()
    {
        if (backgroundMusic.enabled)
        {
            backgroundMusic.enabled = false;
            PlayerPrefs.SetString("backgroundMusicOn", "false");
        }
        else
        {
            backgroundMusic.enabled = true;
            PlayerPrefs.SetString("backgroundMusicOn", "true");
        }
    }

    private void setMusicDefaults()
    {
        if (PlayerPrefs.HasKey("backgroundMusicOn"))
            GlobalAudioScript.Instance.backgroundMusic.enabled = bool.Parse(PlayerPrefs.GetString("backgroundMusicOn"));
    }
}
