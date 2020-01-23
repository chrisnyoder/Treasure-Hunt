using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class ButtonPressAnimation : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    RectTransform rectTransform;

    Vector3 pushedScale;
    Vector3 initialScale;

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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        pushedScale = initialScale * 0.85f;
        transform.DOScale(pushedScale, 1.0f).SetEase(Ease.OutExpo);

    }

}