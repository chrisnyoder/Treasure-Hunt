using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class Slam : MonoBehaviour
{
    public RectTransform rectTransform;
    public Vector3 pushedScale;
    private Vector3 initialScale;

    private void Start() 
    {
        initialScale = rectTransform.localScale;
    }

    public void makeImageBig()
    {
        rectTransform.localScale = initialScale * 2.1f;
    }

    public void animateSlamAnimation()
    {
        var scaleBack = rectTransform.DOScale(initialScale, 1.0f).SetEase(Ease.OutBounce);
        scaleBack.Play();

        GlobalAudioScript.Instance.playSfxSound("Slam_05");
    }
}