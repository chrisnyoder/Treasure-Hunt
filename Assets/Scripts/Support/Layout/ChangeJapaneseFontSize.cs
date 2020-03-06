using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeJapaneseFontSize : MonoBehaviour
{
    void Start()
    {
        if(LocalizationManager.instance.language == SystemLanguage.Japanese)
        {
            var text = GetComponent<Text>();
            text.fontSize = (int)Mathf.Round(text.fontSize * 0.5f);
        }
    }
}
