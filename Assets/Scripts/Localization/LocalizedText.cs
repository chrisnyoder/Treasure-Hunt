using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key;

    private void Start()
    {
        Text text = GetComponent<Text>();
        text.text = LocalizationManager.instance.GetLocalizedText(key);
    }
}
