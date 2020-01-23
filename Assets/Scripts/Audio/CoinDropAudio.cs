using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinDropAudio : MonoBehaviour, IPointerUpHandler

{

    public void OnPointerUp(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound("coin_drop");
    }


   



}