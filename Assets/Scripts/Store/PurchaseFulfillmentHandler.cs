using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseFulfillmentHandler : MonoBehaviour
{
    public GameObject purchaseFailedCanvas;

    private void Start() 
    {
        purchaseFailedCanvas = GameObject.Find("PurchaseFailedCanvas");    
    }

    public void purchaseFulfilled(Product product)
    {
        var productId = gameObject.GetComponent<IAPButton>().productId;
        PlayerPrefs.SetString(productId, "enabled");

        var storeButtonHandler = this.GetComponent<StoreButtonHandler>();
        storeButtonHandler.wordPackProduct.state = ProductState.enabled;

        var storeLayout = this.GetComponentInParent<StoreLayoutScript>();
        storeLayout.displayProductState(this.gameObject);
        storeLayout.populateSelectedWordPacks();
    }

    public void purchaseFailed(Product product, PurchaseFailureReason failureReason)
    {

        if(failureReason == PurchaseFailureReason.UserCancelled || failureReason == PurchaseFailureReason.DuplicateTransaction)
        {
            // do nothing
        } 
        else
        {
            var animator = purchaseFailedCanvas.GetComponent<Animator>();
            animator.Play("StoreInfoPopUpAnimation");
        }
    }
}
