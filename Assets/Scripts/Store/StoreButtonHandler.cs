using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreButtonHandler : MonoBehaviour
{

    public WordPackProduct wordPackProduct;
    public Canvas wordPackProductInfoCanvas;

    public void wordPackStatusChanged()
    {
        var animator = GetComponent<Animator>();
        animator.Play("WordPackSelectedAnimation");

        switch(wordPackProduct.state)
        {
            case ProductState.enabled:
                wordPackProduct.state = ProductState.disabled;
                PlayerPrefs.SetString(wordPackProduct.wordPackProductIdentifier, "disabled");
                print("word pack has been disabled");
                break;
            case ProductState.disabled:
                wordPackProduct.state = ProductState.enabled;
                PlayerPrefs.SetString(wordPackProduct.wordPackProductIdentifier, "enabled");
                print("word pack has been eneabled");
                break;
            case ProductState.unpurchased:
                print("product purchased using the codeless IAP");
                break;
            case ProductState.unavailable:
                print("product unavailable");
                break;
        }

        GlobalAudioScript.Instance.playSfxSound("togglePack2");

        var storeLayout = this.GetComponentInParent<StoreLayoutScript>();
        storeLayout.displayProductState(this.gameObject);
        storeLayout.populateSelectedWordPacks();
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
