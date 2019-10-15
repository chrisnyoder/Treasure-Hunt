using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonChangeColorScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        print("on pointer down called");
        GetComponentInChildren<Text>().color = new Color32(255, 239, 210, 130);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("on pointer up called");
        GetComponentInChildren<Text>().color = new Color32(255, 239, 210, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("on pointer exit called");
        GetComponentInChildren<Text>().color = new Color32(255, 239, 210, 255);
    }

}
