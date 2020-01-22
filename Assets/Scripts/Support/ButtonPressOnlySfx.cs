using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressOnlySfx : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        GlobalAudioScript.Instance.playSfxSound2("click2");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound("click2");
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        GlobalAudioScript.Instance.playSfxSound2("open_swish");

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
