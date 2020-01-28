using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateAudio : MonoBehaviour, IPointerClickHandler

{

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound("pickup");
    }


}