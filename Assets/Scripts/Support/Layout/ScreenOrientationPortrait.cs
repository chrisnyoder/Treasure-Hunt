using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationPortrait : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
