using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEnterAudio : MonoBehaviour, IPointerEnterHandler

{

    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound("pickup");
    }


}
