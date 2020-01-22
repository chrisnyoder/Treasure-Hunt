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

  
        if (ButtonState == true)
        {
            var rotate = transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.Fast);
            ButtonState = false;
            print("Buttonstate is false");
        }

        else 
        {
            var rotateBack = transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.Fast);
            ButtonState = true;
            print("ButtonState is true");
        }



        

    }




}



