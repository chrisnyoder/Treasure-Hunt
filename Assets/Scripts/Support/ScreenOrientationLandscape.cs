using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientationLandscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
