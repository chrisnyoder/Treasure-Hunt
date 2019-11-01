using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatAnimation : MonoBehaviour
{
    public float verticalDistance = 8;
    public float time = 3; 
    public float randomFloat;

    public float radius = 8;
    
    RectTransform rectTransform;
    Vector2 initialPosition; 

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        StartCoroutine(startAnimation()); 
    }

    IEnumerator startAnimation()
    {
        yield return new WaitForSeconds(.1f);
        initialPosition = rectTransform.anchoredPosition;
        getRandomPoint();
    }

    void playAnimation()
    {
        var anim = rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + verticalDistance, time, false);
        anim.SetEase(Ease.Linear);
        anim.SetLoops(-1, LoopType.Yoyo);
        anim.Play();
    }

    void getRandomPoint()
    {
        float a = Random.Range(0f, 1f) * 2f * 3.14f;
        float r = radius * Mathf.Sqrt(Random.Range(0f, 1f));

        int x = (int)(initialPosition.x - r * Mathf.Cos(a));
        float y = (int)(initialPosition.y - r * Mathf.Sin((float)r));

        moveToPoint(new Vector2(x, y));
    }

    void moveToPoint(Vector2 newPoint)
    {
        randomFloat = Random.Range(2, 4);
        var animation = rectTransform.DOAnchorPos(newPoint, randomFloat, false).SetEase(Ease.Linear);   
        animation.Play();
        animation.OnComplete(getRandomPoint);
    }

}
