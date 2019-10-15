using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDefaults : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
       setPlayerDefaults();
       setFrameRate();
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
}
