using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseFulfillmentHandler : MonoBehaviour
{

    public void purchaseFulfilled()
    {
        var productId = gameObject.GetComponent<IAPButton>().productId;
        PlayerPrefs.SetString(productId, "enabled");

        var storeLayout = this.GetComponentInParent<StoreLayoutScript>();
        storeLayout.displayProductState(this.gameObject);
        storeLayout.populateSelectedWordPacks();
    }

    public void purchaseFailed()
    {
        print("purchase failed");
    }

}
