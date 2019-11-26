using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FPLogoAnimation : MonoBehaviour
{
    public float BobbleHeight = 3;
    public float BobbleTime = 1;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(startAnimation());
    }

    IEnumerator startAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        goToInitialPosition();
    }

    public void goToInitialPosition()
    {
        var initialPosition = rectTransform.DOAnchorPosY(0, 1f, false);
        initialPosition.Play();
        initialPosition.OnComplete(startBobbleAnimation);
    }

    void startBobbleAnimation()
    {
        var bobble = rectTransform.DOAnchorPosY(rectTransform.localPosition.y + BobbleHeight, BobbleTime, false).SetEase(Ease.InOutQuad);
        bobble.SetLoops(-1, LoopType.Yoyo);
        bobble.Play();
        
    }
}
