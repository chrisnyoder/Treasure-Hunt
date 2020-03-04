using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour 
{
    public static LocalizationManager instance;

    private string _languageCode;

    [HideInInspector]
    public SystemLanguage language;

    public Dictionary<string, string> localizedText;
    private string missingTextString = "Localized text not found";

    void Awake () 
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        setLanguage();
    }

    public void setLanguage()
    {
        if (PlayerPrefs.HasKey("language"))
        {
            language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), PlayerPrefs.GetString("language"));
        }
        else
        {          
            language = Application.systemLanguage;
            PlayerPrefs.SetString("language", language.ToString());
        }

        _languageCode = Language.getLanguageCode(language);

        LoadLocalizedText(_languageCode);
    }

    private void LoadLocalizedText(string languageCode) 
    {
        print(languageCode);
        localizedText = new Dictionary<string, string> ();

        string fileName = "localizedText_" + languageCode + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath + "/", fileName);

        string dataAsJson;
        
        #if UNITY_EDITOR || UNITY_IOS
        dataAsJson = File.ReadAllText(filePath);

        #elif UNITY_ANDROID 
        UnityWebRequest r = UnityWebRequest.Get(filePath);

        WWW reader = new WWW(filePath);
        while(!reader.isDone){
        }
        string dataAsJson = reader.text;
        #endif

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        
        localizedText.Clear();

        for(int i = 0; i < loadedData.items.Length; i++) {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            print(loadedData.items[i].key);
            print(loadedData.items[i].value);
        }

        print(gameObject.GetInstanceID());

        Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
    }

    public string GetLocalizedText (string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey (key)) {
            result = localizedText[key];
        }
        return result;
    }
}