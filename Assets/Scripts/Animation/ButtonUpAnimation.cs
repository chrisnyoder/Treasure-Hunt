using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class ButtonUpAnimation : MonoBehaviour, IPointerUpHandler  
{
    RectTransform rectTransform;
    Vector3 initialScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        initialScale = rectTransform.localScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        var scaleBack = rectTransform.DOScale(initialScale, 1.0f).SetEase(Ease.OutBounce);
        scaleBack.Play();

    }

}