using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedImage : MonoBehaviour
{
    public string key;

    private void Start()
    {
        string imageName = LocalizationManager.instance.GetLocalizedText(key);
        Sprite localizedImage = Resources.Load<Sprite>("Images/ImagesWithText/" + imageName);

        if(localizedImage != null) 
        {
            Image image = GetComponent<Image>();
            image.sprite = localizedImage;
        }
    }
}
