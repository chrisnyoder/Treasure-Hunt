using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoneEnterAudio : MonoBehaviour, IPointerEnterHandler

{

    public void OnPointerEnter(PointerEventData eventData)
    {
         GlobalAudioScript.Instance.playSfxSound2("stone_drop");
    }


}
