using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class JoinGameHandler : MonoBehaviour
{
    public UIManager uIManager;

    private Scene scene;
    private Role _role;

    public Image roleImage;
    public Text roleText;
    public Text roleDescription;
    public Canvas joinGamePopUpCanvas;
    private Vector2 initialJoinGamePopUpPos;

    public JoinGameNetworkingClient networkingClient; 
    
    public void joinGameAsCaptain()
    {
        _role = Role.captain;
        bringUpJoinGamePopUp(Role.captain);
    }

    public void joingGameAsCrew()
    {
        _role = Role.crew;
        bringUpJoinGamePopUp(Role.crew);
    }

    private void bringUpJoinGamePopUp(Role role)
    {
        networkingClient.role = role;
        
        switch (role)
        {
            case Role.captain:
                roleImage.sprite = Resources.Load<Sprite>("Images/UIElements/captain_button@2x");
                roleText.text = LocalizationManager.instance.GetLocalizedText("team_captain");
                roleDescription.text = LocalizationManager.instance.GetLocalizedText("team_captain_description");
                break;
            case Role.crew:
                roleImage.sprite = Resources.Load<Sprite>("Images/UIElements/crew_button@2x");
                roleText.text = LocalizationManager.instance.GetLocalizedText("crew_member");
                roleDescription.text = LocalizationManager.instance.GetLocalizedText("crew_member_description");
                break;
        }

        GlobalAudioScript.Instance.playSfxSound("openDrawer");

        initialJoinGamePopUpPos = joinGamePopUpCanvas.GetComponent<RectTransform>().anchoredPosition;
        joinGamePopUpCanvas.GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f, false).SetEase(Ease.Linear).Play();
    }

    public void closeJoinGamePopUp()
    {
        GlobalAudioScript.Instance.playSfxSound("closeDrawer");
        joinGamePopUpCanvas.GetComponent<RectTransform>().DOAnchorPosY(initialJoinGamePopUpPos.y, 0.5f, false).SetEase(Ease.Linear).Play();
    }
}
