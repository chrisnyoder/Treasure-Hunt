using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDefaults : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        if(PlayerPrefs.HasKey("appHasStartedBefore"))
        {
            print("app has started before");

            PlayerPrefs.SetString("initialWordList", PlayerPrefs.GetString("initialWordList"));
            PlayerPrefs.SetString("initialWordListExpansion", PlayerPrefs.GetString("initialWordListExpansion"));
            PlayerPrefs.SetString("fantasyWordList", PlayerPrefs.GetString("fantasyWordList"));
            PlayerPrefs.SetString("celebritiesWordList", PlayerPrefs.GetString("celebritiesWordList"));

            PlayerPrefs.SetString("initialWordListJP", PlayerPrefs.GetString("initialWordListJP"));
            PlayerPrefs.SetString("initialWordListExpansionJP", PlayerPrefs.GetString("initialWordListExpansionJP"));

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
