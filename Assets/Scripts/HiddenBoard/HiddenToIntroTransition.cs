using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HiddenToIntroTransition : MonoBehaviour
{
    public UIManager uIManager;
    public RectTransform rt;
    public Image newGameButton; 
    public Image joinGameButton;


    public void startTransition()
    {
        newGameButton.gameObject.SetActive(false);
        joinGameButton.gameObject.SetActive(false);

        rt.DOAnchorPos(new Vector2(0, 0), 0.6f, false).Play().OnComplete(() =>
        {
            uIManager.GoToIntroScreen();
        });
    }
}
