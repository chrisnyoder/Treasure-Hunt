using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateAudio : MonoBehaviour, IPointerDownHandler

{

    public void OnPointerDown(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound("pickup");
    }


}