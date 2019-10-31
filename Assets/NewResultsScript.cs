using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NewResultsScript : MonoBehaviour
{
    public GameObject parentTransform;

    private static List<Vector3> cardPositions = new List<Vector3>()
    {
        new Vector3(227f, -50f, 0f),
        new Vector3(320f, 6f, 0f),
        new Vector3(425f, 56f, 0f),
        new Vector3(535f, 102f, 0f),
        new Vector3(650f, 134f, 0f),
        new Vector3(776f, 159f, 0f),
        new Vector3(902f, 164f, 0f),
        new Vector3(1011f, 132f, 0f)
    };

    private static List<Vector3> cardRotations = new List<Vector3>()
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

    private static List<Vector3> cardScale = new List<Vector3>()
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

    public GameObject card;
    // Start is called before the first frame update
    void Start()
    {
        card.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createCardObjects()
    {
        card.SetActive(true);
        for(int i = 0; i < 8; i++)
        {     
            var cardClone = Instantiate(card, new Vector3(0,0,0), Quaternion.identity, parentTransform.transform);
            var cardCloneRT = cardClone.GetComponent<RectTransform>();
            cardCloneRT.anchoredPosition = cardPositions[0];

            var posAnim = cardCloneRT.DOAnchorPos3D(cardPositions[i], 1f, false);
            posAnim.Play();

            var rotAnim = cardCloneRT.DORotate(cardRotations[i], 1f, RotateMode.Fast);
            rotAnim.Play();

            var scaleAnim = cardCloneRT.DOScale(cardScale[i], 1f);
            scaleAnim.Play();
        }
        card.SetActive(false);
    }

    public void displayResults()
    {
        createCardObjects();
    }  
}
