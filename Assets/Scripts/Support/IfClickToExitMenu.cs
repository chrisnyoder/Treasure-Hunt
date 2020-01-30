using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class IfClickToExitMenu : MonoBehaviour
{
    public MainboardMenuHandler mainboardMenuHandlerRef;
    public RotateAnimation rotateAnimationRef;

    public void ExitIfYouCan()
    {
        if (rotateAnimationRef.MenuIsClosed != true)
        {
            rotateAnimationRef.rotateMenu();
            mainboardMenuHandlerRef.toggleMenu();
            GlobalAudioScript.Instance.playSfxSound2("rotate");
            AudioListener.volume = 0.3f;
            
            print("I'm rotating the arrow back to its starting position");
        }

        else
        {
            print("I did nothing cos the menu wasn't out");
        }
    }
}
