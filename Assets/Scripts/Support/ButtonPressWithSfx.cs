﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class ButtonPressWithSfx : MonoBehaviour,IPointerEnterHandler , IPointerUpHandler, IPointerExitHandler, IPointerDownHandler
{
    RectTransform rectTransform;

    Vector3 pushedScale;
    Vector3 initialScale;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        initialScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        pushedScale = initialScale * 0.85f;
        transform.DOScale(pushedScale, 1.0f).SetEase(Ease.OutExpo);
        GlobalAudioScript.Instance.playSfxSound2("open_swish");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GlobalAudioScript.Instance.playSfxSound("open_swish");
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        GlobalAudioScript.Instance.playSfxSound2("click2");
  
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        var scaleBack = rectTransform.DOScale(initialScale, 1.0f).SetEase(Ease.OutBounce);
        scaleBack.Play();
        //GlobalAudioScript.Instance.playSfxSound2("drop");
  }


}