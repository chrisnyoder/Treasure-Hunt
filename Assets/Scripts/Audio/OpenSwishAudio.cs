using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenSwishAudio: MonoBehaviour, IPointerUpHandler

{

    public void OnPointerUp(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound2("open_swish");
    }


}
