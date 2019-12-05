using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class Slam : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
{




    RectTransform rectTransform;
    Vector3 initialRotation;
    Vector3 pushedScale;
    Vector3 initialScale;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
       
        initialScale = rectTransform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform = GetComponent<RectTransform>();
        pushedScale = initialScale * 2.1f;
        transform.DOScale(pushedScale, 1.0f).SetEase(Ease.InOutBack);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var scaleBack = rectTransform.DOScale(initialScale, 1.0f).SetEase(Ease.OutBounce);
        scaleBack.Play();
    }
}