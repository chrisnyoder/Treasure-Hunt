using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class TimerPressAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform rectTransform;

    Vector3 pushedScale;
    Vector3 initialScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        pushedScale = initialScale * 1.1f;
        transform.DOScale(pushedScale, 1.0f).SetEase(Ease.OutExpo);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        pushedScale = initialScale * 1.0f;
        transform.DOScale(pushedScale, 1.0f).SetEase(Ease.OutExpo);

    }

}
