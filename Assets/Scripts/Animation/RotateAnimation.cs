using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RotateAnimation : MonoBehaviour, IPointerDownHandler
{

    public bool ButtonState = true;
    public void OnPointerDown(PointerEventData eventData)
    {

        if (GlobalDefaults.Instance.isTablet)
        {

            if (ButtonState == true)
            {
                var rotate = transform.DORotate(new Vector3(0, 0, 90), 0.7f, RotateMode.Fast);
                ButtonState = false;
                print("Buttonstate is false");
            }

            else
            {
                var rotateBack = transform.DORotate(new Vector3(0, 0, 270), 0.7f, RotateMode.Fast);
                ButtonState = true;
                print("ButtonState is true");
            }
        }
        else
        {

            if (ButtonState == true)
            {
                var rotate = transform.DORotate(new Vector3(0, 0, 180), 0.7f, RotateMode.Fast);
                ButtonState = false;
                print("Buttonstate is false");
            }

            else
            {
                var rotateBack = transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.Fast);
                ButtonState = true;
                print("ButtonState is true");
            }
        }
    }
}



