using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInfoScript : MonoBehaviour
{

    public void closePopUp()
    {
        var animator = GetComponent<Animator>();
        animator.Play("StoreInfoPopUpAnimationReverse");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
