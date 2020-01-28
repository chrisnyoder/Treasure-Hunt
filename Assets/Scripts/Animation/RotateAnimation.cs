using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class RotateAnimation : MonoBehaviour
{


    public bool MenuIsClosed = true;
    private void Start()
    {
        MenuIsClosed = true;

        }
    
           
    public void rotateMenu() 
    {

        Tween rotate;
        GetComponent<Button>().enabled = false;
        

        if (GlobalDefaults.Instance.isTablet)
        {

            if (MenuIsClosed == true)
            {
                rotate = transform.DORotate(new Vector3(0, 0, 90), 0.7f, RotateMode.Fast);
                MenuIsClosed = false;
                print("menu is open");
            }

            else
            {
                rotate = transform.DORotate(new Vector3(0, 0, 270), 0.7f, RotateMode.Fast);
                MenuIsClosed = true;
                print("menu is closed");
            }
        }
        else
        {

            if (MenuIsClosed == true)
            {
                rotate = transform.DORotate(new Vector3(0, 0, 180), 0.7f, RotateMode.Fast);
                MenuIsClosed = false;
                print("menu is open");
            }

            else
            {
                rotate = transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.Fast);
                MenuIsClosed = true;
                print("menu is closed");
            }
        }
        rotate.OnComplete(() =>
        {

            GetComponent<Button>().enabled = true;

        });
    }
}



