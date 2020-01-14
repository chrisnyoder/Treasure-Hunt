using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayHandler : MonoBehaviour
{
    private GameState _gameState; 

    public VerticalLayoutGroup blueVert; 
    public VerticalLayoutGroup redVert; 

    public GameObject blueCoinPrefab;
    public GameObject redCoinPrefab;

    private List<GameObject> blueScorePrefabList = new List<GameObject>();
    private List<GameObject> redScorePrefabList = new List<GameObject>();

    public void receiveInitialGameState(GameState gameState)
    {
        resetScore();
        _gameState = gameState;

        instantiateScorePrefabs();
    }

    private void resetScore()
    {
        foreach(var score in blueScorePrefabList)
        {
            Destroy(score);
        }

        foreach(var score in redScorePrefabList)
        {
            Destroy(score);
        }

        blueScorePrefabList.Clear();
        redScorePrefabList.Clear();
    }

    private void instantiateScorePrefabs()
    {
        for(var i = 0; i < _gameState.numberOfBlueCards; ++i)
        {
            GameObject blueScoreClone = Instantiate(blueCoinPrefab, new Vector3(0, 0, 0), Quaternion.identity, blueVert.transform);
            var coinImages = blueScoreClone.GetComponentsInChildren<Image>();
            var imageNumber = i + 1;
            coinImages[1].sprite = Resources.Load<Sprite>("Images/MainBoard/score_coin_" + imageNumber);
            coinImages[1].enabled = false;
            blueScorePrefabList.Add(blueScoreClone);
        }

        for(var i = 0; i < _gameState.numberOfRedCards; ++i)
        {
            GameObject redScoreClone = Instantiate(redCoinPrefab, new Vector3(0, 0, 0), Quaternion.identity, redVert.transform);
            var coinImages = redScoreClone.GetComponentsInChildren<Image>();
            var imageNumber = i + 1;
            coinImages[1].sprite = Resources.Load<Sprite>("Images/MainBoard/score_coin_" + imageNumber);
            coinImages[1].enabled = false;
            redScorePrefabList.Add(redScoreClone);
        }

        displayScore();
    }

    public void displayScore()
    {
        for(var i = 0; i < _gameState.blueTeamScore; ++i)
        {
            print("score in button handler: " + _gameState.blueTeamScore);
            var coin = blueScorePrefabList[i].GetComponentsInChildren<Image>()[1];
            coin.enabled = true; 
        }

        for (var i = 0; i < _gameState.redTeamScore; ++i)
        {
            print("score in button handler: " + _gameState.redTeamScore);
            var coin = redScorePrefabList[i].GetComponentsInChildren<Image>()[1];
            coin.enabled = true;
        }
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
