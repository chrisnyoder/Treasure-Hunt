using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EoGScript : MonoBehaviour
{

    private GameObject eogImage;
    public Network network; 

    public Sprite blueWins;
    public Sprite redWins;
    public Sprite lose;

    public void DisplayEOGImage(GameState gameState)
    {
        eogImage = GameObject.Find("EndOfGameImage");
        var image = eogImage.GetComponent<Image>();

        var currentGameState = gameState.currentGameState;

        switch (currentGameState)
        {
            case CurrentGameState.blueWins:
                GlobalAudioScript.Instance.playSfxSound("win_sfx");
                image.sprite = blueWins;
                break;
            case CurrentGameState.redWins:
                GlobalAudioScript.Instance.playSfxSound("win_sfx");
                image.sprite = redWins;
                break;
            case CurrentGameState.loses:
                image.sprite = lose;
                break;
        }

        var animator = GetComponent<Animator>();
        animator.Play("ResultsCanvasAnimation");
    }
}
