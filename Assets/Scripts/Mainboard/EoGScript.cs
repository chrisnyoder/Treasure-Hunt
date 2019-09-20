using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EoGScript : MonoBehaviour
{

    private GameObject eogImage;

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
                image.sprite = blueWins;
                break;
            case CurrentGameState.redWins:
                image.sprite = redWins;
                break;
            case CurrentGameState.loses:
                image.sprite = lose;
                break;
        }

        var canvasRT = GetComponent<RectTransform>();
        canvasRT.localPosition = new Vector2(canvasRT.localPosition.x, 0);
    }
}
