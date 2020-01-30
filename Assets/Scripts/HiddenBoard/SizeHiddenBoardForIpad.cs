using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeHiddenBoardForIpad : MonoBehaviour
{
    public RectTransform hiddenBoardRect; 

    private void Awake() 
    {
        if(GlobalDefaults.Instance.isTablet)
        {
            hiddenBoardRect.localScale = hiddenBoardRect.localScale * 0.7f;
        }
    }
}
