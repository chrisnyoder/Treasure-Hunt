using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonHandler : MonoBehaviour
{

    public WordPackProduct wordPackProduct;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void wordPackStatusChanged()
    {
        switch(wordPackProduct.state)
        {
            case ProductState.enabled:
                wordPackProduct.state = ProductState.disabled;
                print("word pack has been disabled");
                break;
            case ProductState.disabled:
                wordPackProduct.state = ProductState.enabled;
                print("word pack has been eneabled");
                break;
            case ProductState.unpurchased:
                print("purchase products");
                break;
            case ProductState.unavailable:
                print("product unavailable");
                break;
        }
        var storeLayout = this.GetComponentInParent<StoreLayoutScript>();
        storeLayout.displayProductState(this.gameObject);
        storeLayout.populateSelectedWordPacks();
    }
}
