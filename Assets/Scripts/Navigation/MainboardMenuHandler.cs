using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainboardMenuHandler : MonoBehaviour
{
    public RectTransform pulloutIcon;
    public RectTransform musicIcon;
    public RectTransform pauseIcon;
    public RectTransform exitIcon;
    public RectTransform restartIcon;

    private void Awake() 
    {
        if(GlobalDefaults.Instance.isTablet)
        {
            pulloutIcon.transform.Rotate(0, 0, 270);
        }
    }

    void Start()
    {
        musicIcon.anchoredPosition = new Vector2(0, 0);
        pauseIcon.anchoredPosition = new Vector2(0, 0);
        exitIcon.anchoredPosition = new Vector2(0, 0);
        restartIcon.anchoredPosition = new Vector2(0, 0);
    }

    public void toggleMenu()
    {
        if(GlobalDefaults.Instance.isTablet)
        {
            if (musicIcon.anchoredPosition.x == 0)
            {
                Sequence s = DOTween.Sequence();
                s.Join(musicIcon.DOAnchorPosX(-130, 0.3f, false));
                s.Join(pauseIcon.DOAnchorPosX(-250, 0.3f, false));
                s.Join(restartIcon.DOAnchorPosX(-370, 0.3f, false));
                s.Join(exitIcon.DOAnchorPosX(- 490, 0.3f, false));
            }
            else
            {
                Sequence s = DOTween.Sequence();
                s.Join(musicIcon.DOAnchorPosX(0, 0.3f, false));
                s.Join(pauseIcon.DOAnchorPosX(0, 0.3f, false));
                s.Join(restartIcon.DOAnchorPosX(0, 0.3f, false)); 
                s.Join(exitIcon.DOAnchorPosX(0, 0.3f, false));
            }
        }
        else
        {
            if (musicIcon.anchoredPosition.y == 0)
            {
                Sequence s = DOTween.Sequence();
                s.Join(musicIcon.DOAnchorPosY(-130, 0.3f, false));
                s.Join(pauseIcon.DOAnchorPosY(-250, 0.3f, false));
                s.Join(restartIcon.DOAnchorPosY(-370, 0.3f, false));
                s.Join(exitIcon.DOAnchorPosY(-490, 0.3f, false));
            }
            else
            {
                Sequence s = DOTween.Sequence();
                s.Join(musicIcon.DOAnchorPosY(0, 0.3f, false));
                s.Join(pauseIcon.DOAnchorPosY(0, 0.3f, false));
                s.Join(restartIcon.DOAnchorPosY(0, 0.3f, false));
                s.Join(exitIcon.DOAnchorPosY(0, 0.3f, false));
            }
        }    
    }
}
