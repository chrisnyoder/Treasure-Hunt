using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class ButtonExitAnimation : MonoBehaviour, IPointerExitHandler
{
    RectTransform rectTransform;
    Vector3 initialScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var scaleBack = rectTransform.DOScale(initialScale, 1.0f).SetEase(Ease.OutBounce);
        scaleBack.Play();

    }

}
