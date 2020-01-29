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

    void Start()
    {
        rotateAnimationRef = GameObject.Find("MenuPulloutToggle").GetComponent<RotateAnimation>();
        mainboardMenuHandlerRef = GameObject.Find("MenuPulloutParent").GetComponent<MainboardMenuHandler>();
    }

    public void ExitIfYouCan()
    {
        if (rotateAnimationRef.MenuIsClosed != true)
        {
            rotateAnimationRef.rotateMenu();
            mainboardMenuHandlerRef.toggleMenu();
            
            print("I'm rotating the arrow back to its starting position");
        }

        else
        {
            print("I did nothing cos the menu wasn't out");
        }

    }

    // Start is called before the first frame update




}
