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

    private int iconDistanceFromOrigin = -130;
    private List<RectTransform> icons; 

    private void Awake() 
    {
        gameObject.SetActive(false);

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

        icons = new List<RectTransform>() {musicIcon, pauseIcon, exitIcon};

        if(restartIcon != null)
        {
            restartIcon.anchoredPosition = new Vector2(0, 0);
            icons.Insert(icons.Count-1, restartIcon);
        }
    }

    public void toggleMenu()
    {
        Sequence s = DOTween.Sequence();

        if(GlobalDefaults.Instance.isTablet)
        {
            if (musicIcon.anchoredPosition.x == 0)
            {
                foreach(RectTransform icon in icons)
                {
                    s.Join(icon.DOAnchorPosX(iconDistanceFromOrigin, 0.3f, false));
                    iconDistanceFromOrigin -= 130;
                }
            }
            else
            {
                foreach (RectTransform icon in icons)
                {
                    s.Join(icon.DOAnchorPosX(0, 0.3f, false));
                }
            }
        }
        else
        {
            if (musicIcon.anchoredPosition.y == 0)
            {
                foreach (RectTransform icon in icons)
                {
                    s.Join(icon.DOAnchorPosY(iconDistanceFromOrigin, 0.3f, false));
                    iconDistanceFromOrigin -= 130;
                }
            }
            else
            {
                foreach (RectTransform icon in icons)
                {
                    s.Join(icon.DOAnchorPosY(0, 0.3f, false));
                }
            }
        }
        iconDistanceFromOrigin = -130;
    }
}
