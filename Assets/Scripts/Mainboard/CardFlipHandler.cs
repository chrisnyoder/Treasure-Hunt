using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardFlipHandler : MonoBehaviour
{
    private EoGScript eoGScript;
    public bool cardIsFlipped;
    public GameState gameState;

    public CardType cardType;
    public string cardText;

    private WSNetworkingClient networkingClient;

    private void Awake() 
    {
        networkingClient = GameObject.Find("NetworkingClient").GetComponent<WSNetworkingClient>();
    }

    public void FlipCard()
    {
        cardIsFlipped = true;
        print("flip card function being called");

        if(gameState.currentGameState == CurrentGameState.gameInPlay)
        {
            switch (cardType)
            {
                case CardType.blueCard:
                    GlobalAudioScript.Instance.playSfxSound("coin_flip");
                    gameState.blueTeamScore += 1;
                    if (gameState.blueTeamScore >= 8)
                    {
                        gameState.currentGameState = CurrentGameState.blueWins;
                    }
                    break;
                case CardType.redCard:
                    GlobalAudioScript.Instance.playSfxSound("correct");
                    gameState.redTeamScore += 1;
                    if (gameState.redTeamScore >= 7)
                    {
                        gameState.currentGameState = CurrentGameState.redWins;
                    }
                    break;
                case CardType.neutralCard:
                    GlobalAudioScript.Instance.playSfxSound("flip_stone");
                    break;
                case CardType.shipwreckCard:
                    GlobalAudioScript.Instance.playSfxSound("sad_violin");
                    gameState.currentGameState = CurrentGameState.loses;
                    break;
            }

            Destroy(gameObject.GetComponentInChildren<Text>());
            gameObject.GetComponent<Button>().interactable = false;

            var rt = gameObject.GetComponent<RectTransform>();
            Sequence s = DOTween.Sequence();

            s.Append(rt.DORotate(new Vector3(0f, 90f, 0), 0.3f, RotateMode.Fast));
            s.Join(rt.DOScale(new Vector3(1.2f, 0.8f, 1), 0.3f));

            s.Play().OnComplete(changeCardColor);
        }
    }

    public void changeCardColor()
    {

        switch (cardType)
        {
            case CardType.blueCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/blue_card");
                break;
            case CardType.redCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/red_card"); 
                break;
            case CardType.neutralCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/sand_card"); 
                break;
            case CardType.shipwreckCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/Shipwreck_Card");
                break;
        }

        if(gameState.currentGameState != CurrentGameState.gameInPlay)
        {
            StartCoroutine(LaunchEoGAfterDelay());
        }

        gameState.wordsAlreadySelected.Add(cardText);
        networkingClient.wordsSelectedQueue.Add(cardText);

        var rt = gameObject.GetComponent<RectTransform>();
        Sequence s = DOTween.Sequence();

        s.Append(rt.DORotate(new Vector3(0f, 180f, 0), 0.3f, RotateMode.Fast));
        s.Join(rt.DOScale(new Vector3(1f, 1f, 1), 0.3f));

        s.Play();
        print("changing color");
    }

    public void startCardFaceUp()
    {
        switch (cardType)
        {
            case CardType.blueCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/blue_card");
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, -180f, 0f); 
                break;
            case CardType.redCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/red_card");
                GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, -180, 0)) ;
                break;
            case CardType.neutralCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/sand_card");
                GetComponent<RectTransform>().localRotation = new Quaternion(0, -180, 0, 0);
                break;
            case CardType.shipwreckCard:
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/MainBoard/Shipwreck_Card");
                GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, -180f, 0f);
                break;
        }

        Destroy(gameObject.GetComponentInChildren<Text>());
        gameObject.GetComponent<Button>().interactable = false;
        cardIsFlipped = true;
    }

    IEnumerator LaunchEoGAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameState.LaunchEOGScreen();
        print("current game state is: " + gameState.currentGameState);
        networkingClient.sendCurrentGameState(gameState.currentGameState);
    }
}
