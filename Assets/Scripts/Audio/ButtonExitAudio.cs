using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonExitAudio : MonoBehaviour, IPointerExitHandler

{

    public void OnPointerExit(PointerEventData eventData)
    {

    
        GlobalAudioScript.Instance.playSfxSound2("drop");
    }

}
