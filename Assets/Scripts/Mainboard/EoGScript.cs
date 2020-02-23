using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EoGScript : MonoBehaviour
{
    public GameObject card;
    public List<GameObject> cards = new List<GameObject>(){};
    public GameObject resultsCollectionView;

    public GameObject loseBanner;
    public GameObject winBanner;
    public GameObject gameOverText;
    public Canvas rateTheAppCanvas; 

    public Slam slam;

    private CurrentGameState currentGameState;
    private int _playCounter; 

    private void Start() {
        card.SetActive(false);
        loseBanner.SetActive(false);
        winBanner.SetActive(false);
        rateTheAppCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 3000);
    }

    public void DisplayEOGCanvas(CurrentGameState currentGameState)
    {

        if(cards.Count > 0)
        {
            foreach (GameObject card in cards)
            {
                Destroy(card);
            }
        }

        gameOverText.SetActive(false);

        this.currentGameState = currentGameState;

        var loseImage = loseBanner.GetComponent<Image>();
        var winImage = winBanner.GetComponent<Image>();

        switch (currentGameState)
        {
            case CurrentGameState.blueWins:
                winBanner.SetActive(true);
                loseBanner.SetActive(false);
                GlobalAudioScript.Instance.playSfxSound("win_sfx");
                winImage.sprite = Resources.Load<Sprite>("Images/ImagesWithText" + LocalizationManager.instance.GetLocalizedText("game_win_blue"));
                card.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Results/blue_card@2x");
                break;
            case CurrentGameState.redWins:
                winBanner.SetActive(true);
                loseBanner.SetActive(false);
                GlobalAudioScript.Instance.playSfxSound("win_sfx");
                winImage.sprite = Resources.Load<Sprite>("Images/ImagesWithText" + LocalizationManager.instance.GetLocalizedText("game_win_red"));
                card.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Results/red_card@2x");
                break;
            case CurrentGameState.loses:
                slam.makeCoinBig();
                winBanner.SetActive(false);
                loseBanner.SetActive(true);
                gameOverText.SetActive(true);
                break;
        }

        var rt = GetComponent<RectTransform>();
        rt.DOAnchorPosY(0, 0.7f).Play().OnComplete(() => {
            playEoGCardAnimations();
            rt.GetComponent<Image>().DOFade(0.627f, 0.3f);
        });
        
    }

    private void playEoGCardAnimations()
    {
        print("animations being played");

        if(currentGameState == CurrentGameState.blueWins || currentGameState == CurrentGameState.redWins)
        {
            var positions = EoGCardAnimationData.cardPositions;
            var rotations = EoGCardAnimationData.cardRotations;
            var scale = EoGCardAnimationData.cardScale;

            for (int i = 0; i < positions.Count; i++)
            {
                card.SetActive(true);
                var cardClone = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, resultsCollectionView.transform);
                var cardCloneRT = cardClone.GetComponent<RectTransform>();
                cardCloneRT.anchoredPosition = positions[0];

                var posAnim = cardCloneRT.DOAnchorPos3D(positions[i], 1f, false);
                posAnim.Play();

                var rotAnim = cardCloneRT.DORotate(rotations[i], 1f, RotateMode.Fast);
                rotAnim.Play();

                var scaleAnim = cardCloneRT.DOScale(scale[i], 1f);
                scaleAnim.Play();
                cards.Add(cardClone);
            }
            card.SetActive(false);
        } else if (currentGameState == CurrentGameState.loses) 
        {
            slam.animateEoGCursedCoin();
        }
        determineGameCount();
    }

    void determineGameCount()
    {
        if (PlayerPrefs.HasKey("gamesPlayed"))
        {
            string previousCounter = PlayerPrefs.GetString("gamesPlayed");
            int newCounter = int.Parse(previousCounter) + 1;
            _playCounter = newCounter;

            PlayerPrefs.SetString("gamesPlayed", newCounter.ToString());
        }
        else
        {
            PlayerPrefs.SetString("gamesPlayed", "0");
            _playCounter = 0;
        }

        if (_playCounter == 1)
        {
            callForReview();
        }
    }

    void callForReview()
    {
        #if UNITY_IOS
            UnityEngine.iOS.Device.RequestStoreReview();
        #elif UNITY_ANDROID
            rateTheAppCanvas.GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f).SetDelay(1f).Play();
        #endif
    }

    public void rateAndroidGame()
    {
        var url = "https://play.google.com/store/apps/details?id=com.FriendlyPixel.TreasureHunt";
        Application.OpenURL(url);
    }
}


public struct EoGCardAnimationData
{
    public static List<Vector3> cardPositions = new List<Vector3>()
    {
        new Vector3(-478f, -90f, 0f),
        new Vector3(-385f, -18f, 0f),
        new Vector3(-281f, 42f, 0f),
        new Vector3(-181f, 88f, 0f),
        new Vector3(-87f, 122f, 0f),
        new Vector3(21f, 150f, 0f),
        new Vector3(129f, 170f, 0f),
        new Vector3(256f, 185f, 0f)
    };

    public static List<Vector3> cardRotations = new List<Vector3>()
    {
        new Vector3(0f, 0f, 35f),
        new Vector3(0f, 0f, 26f),
        new Vector3(0f, 0f, 22f),
        new Vector3(0f, 0f, 17f),
        new Vector3(0f, 0f, 12f),
        new Vector3(0f, 0f, 6f),
        new Vector3(0f, 0f, -8f),
        new Vector3(0f, 0f, -30f)
    };

    public static List<Vector3> cardScale = new List<Vector3>()
    {
        new Vector3(.78f, .78f, .78f),
        new Vector3(.82f, .82f, .82f),
        new Vector3(.84f, .84f, .84f),
        new Vector3(.85f, .85f, .85f),
        new Vector3(.87f, .87f, .87f),
        new Vector3(.9f, .9f, .9f),
        new Vector3(1f, 1f, 1f),
        new Vector3(1.2f, 1.2f, 1.2f)
    };
}