using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ChangeInfoScreenLayout : MonoBehaviour
{
    public GridLayoutGroup grid; 

    void Start()
    {
        if(GlobalDefaults.Instance.isTablet)
        {
            grid.cellSize = new Vector2(140, 200);
        }
    }
}
