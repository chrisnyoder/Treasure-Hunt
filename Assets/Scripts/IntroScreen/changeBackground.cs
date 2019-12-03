﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeBackground : MonoBehaviour
{
    void Awake() 
    {
        if(GlobalDefaults.Instance.isTablet)
        {

            Debug.Log("changing background", this.gameObject);
            var backgroundImage = this.GetComponent<Image>();
            backgroundImage.sprite = Resources.Load<Sprite>("Images/Backgrounds/Full_Background_ipad_12@2x");
        }
    }
}
