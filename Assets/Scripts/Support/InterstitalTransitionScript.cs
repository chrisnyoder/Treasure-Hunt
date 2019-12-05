using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InterstitalTransitionScript : MonoBehaviour
{
    public RectTransform rt;
    public UIManager uIManager;
    public Image newGameButton; 
    public Image joinGameButton; 

    private void Awake() 
    {
        rt.anchoredPosition = new Vector2(0, 0);    
    }

    // Start is called before the first frame update
    void Start()
    {
        rt.DOAnchorPosY(-2000, 1.5f, false).Play();
    }

    public void reverseTransition()
    {
        GlobalAudioScript.Instance.playSfxSound("openMenu");

        newGameButton.gameObject.SetActive(false);
        joinGameButton.gameObject.SetActive(false);

        rt.DOAnchorPosY(0, 0.6f, false).Play().OnComplete(() => 
        {
            uIManager.GoToIntroScreen();
        });
    }
}
