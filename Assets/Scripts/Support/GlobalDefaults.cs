using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDefaults : MonoBehaviour
{

    private static GlobalDefaults _instance; 
    private static bool appSettingsSet = false;

    public static GlobalDefaults Instance  
    {
        get 
        {
            return _instance;
        }
    }

    public RectTransform screenSize; 
    public bool isTablet; 

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
    private void Start() 
    {
        if(!appSettingsSet)
        {
            setPlayerDefaults();
            setFrameRate();
            determineIfTablet();
        }  
        appSettingsSet = true;
    }

    void setPlayerDefaults()
    {
        PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey("appHasStartedBefore"))
        {
            print("app has started before");

            var allWordPackIds = WordPackProductIdentifiers.returnAllProucts();

            foreach (string wp in allWordPackIds)
            {
                PlayerPrefs.SetString(wp, PlayerPrefs.GetString(wp));
            }
        }
        else
        {
            PlayerPrefs.SetString("appHasStartedBefore", "true");
            print("app is starting up for first time");

            PlayerPrefs.SetString("initialWordList", "enabled");
            PlayerPrefs.SetString("initialWordListExpansion", "unpurchased");
            PlayerPrefs.SetString("fantasyWordList", "unpurchased");
            PlayerPrefs.SetString("celebritiesWordList", "unpurchased");

            PlayerPrefs.SetString("initialWordListJP", "disabled");
            PlayerPrefs.SetString("initialWordListExpansionJP", "unpurchased");
        }
    }

    void setFrameRate()
    {
        Application.targetFrameRate = 60;
    }

    void determineIfTablet()
    {
        var screenWidth = screenSize.rect.width;
        var screenHeight = screenSize.rect.height; 
        var aspectRatio = screenWidth/screenHeight;

        if(aspectRatio <= 1.4)
        {
            isTablet = true;
        } 
        else
        {
            isTablet = false;
        }
    }
}
