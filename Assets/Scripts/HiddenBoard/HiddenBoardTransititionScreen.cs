using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HiddenBoardTransititionScreen : MonoBehaviour
{

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.color = Color.black;
        fadeTransition();
    }

    void fadeTransition()
    {
        var anim = image.DOFade(0, 0.5f);
        anim.SetOptions(true);
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
