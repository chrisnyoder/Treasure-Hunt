﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainboardMenuHandler : MonoBehaviour
{
    public RectTransform musicIcon;
    public RectTransform pauseIcon;
    public RectTransform exitIcon; 

    // Start is called before the first frame update
    void Start()
    {
        musicIcon.anchoredPosition = new Vector2(0, 0);
        pauseIcon.anchoredPosition = new Vector2(0, 0);
        exitIcon.anchoredPosition = new Vector2(0, 0);
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
                s.Join(exitIcon.DOAnchorPosX(-370, 0.3f, false));
            }
            else
            {
                Sequence s = DOTween.Sequence();
                s.Join(musicIcon.DOAnchorPosX(0, 0.3f, false));
                s.Join(pauseIcon.DOAnchorPosX(0, 0.3f, false));
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
                s.Join(exitIcon.DOAnchorPosY(-370, 0.3f, false));
            }
            else
            {
                Sequence s = DOTween.Sequence();
                s.Join(musicIcon.DOAnchorPosY(0, 0.3f, false));
                s.Join(pauseIcon.DOAnchorPosY(0, 0.3f, false));
                s.Join(exitIcon.DOAnchorPosY(0, 0.3f, false));
            }
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}