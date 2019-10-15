using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class InfoScript : MonoBehaviour
{

    public Canvas InfoPopUp;

    public void bringUpInfoPopUp()
    {
        var animator = InfoPopUp.GetComponent<Animator>();
        animator.Play("StoreInfoPopUpAnimation");
    }

    public void closePopUp()
    {
        var animator = GetComponent<Animator>();
        animator.Play("StoreInfoPopUpAnimationReverse");
    }

    public void contactFriendlyPixel()
    {
        string email = "contact@friendlypixel.app";
        string subject = MyEscapeURL("Subject");
        string body = MyEscapeURL("");

        print("mailto:" + email + "?subject=" + subject + "%body=" + body);

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private string MyEscapeURL(string URL)
    {
        return UnityWebRequest.EscapeURL(URL).Replace("+", "%20");
    }

    public void bringUpPrivacyPolicy()
    {
        Application.OpenURL("https://friendlypixel.app/treasure_hunt_privacy_policy.html");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

