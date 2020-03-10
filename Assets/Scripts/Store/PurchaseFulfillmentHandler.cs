using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class PurchaseFulfillmentHandler : MonoBehaviour
{
    public GameObject purchaseFailedCanvas;
    public Text failedReasonText; 

    private void Start() 
    {
        purchaseFailedCanvas = GameObject.Find("PurchaseFailedCanvas");    
    }

    public void purchaseFulfilled(Product product)
    {
        print("purchase fulfilled being called for product: " + product.definition.id);
        
        var productId = product.definition.id;
        PlayerPrefs.SetString(productId, "enabled");

        var storeButtonHandler = this.GetComponent<StoreButtonHandler>();
        if(storeButtonHandler != null) 
        {
            storeButtonHandler.wordPackProduct.state = ProductState.enabled;
        }
        
        var storeLayout = this.GetComponentInParent<StoreLayoutScript>();
        if(storeLayout != null)
        {
            storeLayout.displayProductState(this.gameObject);
            storeLayout.populateSelectedWordPacks();  
        }
    }

    public void purchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("purchase failed");

        if(failureReason == PurchaseFailureReason.UserCancelled)
        {
            // do nothing 
        } 
        else if(failureReason == PurchaseFailureReason.DuplicateTransaction)
        {
            if (purchaseFailedCanvas != null)
            {
                failedReasonText.text = "Looks like this purchase is a duplicate. It can be restored by through the app on iOS and Google Play on Android";
                var animator = purchaseFailedCanvas.GetComponent<Animator>();
                animator.Play("StoreInfoPopUpAnimation");
            }
        } 
        else
        {
            if(purchaseFailedCanvas != null)
            {
                var animator = purchaseFailedCanvas.GetComponent<Animator>();
                animator.Play("StoreInfoPopUpAnimation");
            }
        }
    }
}
