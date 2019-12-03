using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackTransitionScript : MonoBehaviour
{
    public Image transitionImage; 
    
    private void Awake() 
    {
        transitionImage.color = new Color(0, 0, 0, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        transitionImage.DOFade(0, 0.2f).Play();
    }
}
