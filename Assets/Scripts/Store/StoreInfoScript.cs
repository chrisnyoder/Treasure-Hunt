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
}
