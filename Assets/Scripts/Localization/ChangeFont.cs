using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFont : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(LocalizationManager.instance.language == SystemLanguage.Japanese)
        {
            Text[] texts = GetComponentsInChildren<Text>();

            foreach(Text text in texts)
            {
                text.font = Resources.Load<Font>("Fonts/NotoSansJP-Bold");
            }  
        }   
    }
}
