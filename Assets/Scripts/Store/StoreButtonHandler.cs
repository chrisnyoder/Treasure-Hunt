using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoreButtonHandler : MonoBehaviour
{
    private Tween anim;
    public RectTransform rectTransform;    
    public WordPackProduct wordPackProduct;
    public Canvas wordPackProductInfoCanvas;

    private void Start() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void wordPackStatusChanged()
    {
        wordPackSelectedAnimation();

        switch(wordPackProduct.state)
        {
            case ProductState.enabled:
                wordPackProduct.state = ProductState.disabled;
                PlayerPrefs.SetString(wordPackProduct.wordPackProductIdentifier, "disabled");
                break;
            case ProductState.disabled:
                wordPackProduct.state = ProductState.enabled;
                PlayerPrefs.SetString(wordPackProduct.wordPackProductIdentifier, "enabled");
                break;
            case ProductState.unpurchased:
                break;
            case ProductState.unavailable:
                break;
        }

        GlobalAudioScript.Instance.playSfxSound("togglePack2");

        var storeLayout = this.GetComponentInParent<StoreLayoutScript>();
        storeLayout.displayProductState(this.gameObject);
        storeLayout.populateSelectedWordPacks();
    }

    private void wordPackSelectedAnimation()
    {
        anim = rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f);
        gameObject.GetComponent<Button>().interactable = false;
        anim.SetAutoKill(false);
        anim.Play(); 
        anim.OnComplete(unwindAnimation);
    }

    private void unwindAnimation()
    {
        anim.SmoothRewind();
        anim.OnRewind(makeButtonInteractable);
    }

    private void makeButtonInteractable()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void infoButtonSelected()
    {
        var productImage = wordPackProductInfoCanvas.GetComponentsInChildren<Image>();
        productImage[2].sprite = wordPackProduct.wordPackImage;

        var txtTitle = wordPackProductInfoCanvas.GetComponentsInChildren<Text>();
        txtTitle[0].text = wordPackProduct.wordPackProductTitle;

        var txtDescription = wordPackProductInfoCanvas.GetComponentsInChildren<Text>();
        txtDescription[1].text = wordPackProduct.wordPackDescription;

        GlobalAudioScript.Instance.playSfxSound("openDrawer");

        var animator = wordPackProductInfoCanvas.GetComponent<Animator>();
        animator.Play("StoreInfoPopUpAnimation");
    }
}
