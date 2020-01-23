using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoneExitAudio : MonoBehaviour, IPointerExitHandler

{

    public void OnPointerExit(PointerEventData eventData)
    {
       // GlobalAudioScript.Instance.playSfxSound3("drop");
    }


}
