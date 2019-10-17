using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class StoreLayoutScript : MonoBehaviour
{
    public GameObject storeCollectionView;
    public GameObject wordPackButton; 
    public ProductLanguage languageSelected = ProductLanguage.English; 
    public GameObject restorePurchaseButton;
    
    List<WordPackProduct> wordPacksToSelect = new List<WordPackProduct>{};
    List<WordPackProduct> selectedWordPacks = new List<WordPackProduct> { };

    public Sprite starterWordPackImage;
    public Sprite expansionWordPackImage;
    public Sprite fantasyWordPackImage;
    public Sprite celebrityWordPackImage;

    public Sprite enabledStar; 
    public Sprite disabledStar; 
    public Sprite lockImage; 
    
    // Start is called before the first frame update
    void Start()
    {
        var storeCanvasRT = this.gameObject.GetComponent<RectTransform>();

        storeCanvasRT.localPosition = new Vector3(storeCanvasRT.localPosition.x, 0, 0);

        loadWordPacks();

        #if UNITY_IOS
            restorePurchaseButton.SetActive(true);
        #elif UNITY_EDITOR
            restorePurchaseButton.SetActive(true);
        #else 
            restorePurchaseButton.SetActive(false);
        #endif
    }

    public void loadWordPacks()
    {
        wordPacksToSelect.Clear();
        var allWordPacks = WordPackProductIdentifiers.returnAllProucts();

        foreach(string wordpackidentifier in allWordPacks)
        {
            var wordPackObject = new WordPackProduct(wordpackidentifier);

            if(wordPackObject.language == languageSelected)
            {
                wordPacksToSelect.Add(wordPackObject);
            }
        }

        instantiateWordPackPrefabsAndPlaceImages();
    }

    public void instantiateWordPackPrefabsAndPlaceImages()
    {
        var wordPackPosition = new Vector3(0, 0, 0);
        
        var storeCollectionViewRT = storeCollectionView.GetComponent<RectTransform>();
        var wordPackWidth = (storeCollectionViewRT.rect.width*0.75f)/4;
        var emptySpace = (storeCollectionViewRT.rect.width - (wordPackWidth*wordPacksToSelect.Count))/(wordPacksToSelect.Count+1);
        float xCardPosition = emptySpace + (wordPackWidth/2); 

        foreach(WordPackProduct wordPack in wordPacksToSelect)
        {
            GameObject wordPackClone = Instantiate(wordPackButton, wordPackPosition, Quaternion.identity, storeCollectionView.transform);

            Image wordPackCloneImage = wordPackClone.GetComponent<Image>();
            wordPackClone.GetComponent<StoreButtonHandler>().wordPackProduct = wordPack;

            wordPackCloneImage.sprite = wordPack.wordPackImage;

            var wordPackCloneRT = wordPackClone.GetComponent<RectTransform>();
            wordPackCloneRT.sizeDelta = new Vector2(wordPackWidth, wordPackWidth * 1.7f);
            wordPackCloneRT.anchoredPosition = new Vector3(xCardPosition, 0 , 0);

            xCardPosition += wordPackWidth+emptySpace; 
            displayProductState(wordPackClone);
        }    

        GameObject.Destroy(wordPackButton.gameObject, 0f);
        populateSelectedWordPacks();
    }

    public void displayProductState(GameObject wordPackClone)
    {

        Image star = wordPackClone.transform.GetChild(0).GetComponent<Image>();
        var wordPackData = wordPackClone.GetComponent<StoreButtonHandler>().wordPackProduct;

        switch (wordPackData.state)
        {
            case ProductState.enabled:
                star.sprite = enabledStar;
                if (wordPackClone.GetComponent<IAPButton>() != null)
                    StartCoroutine(disableIAPButton(wordPackClone));
                break;
            case ProductState.disabled:
                star.sprite = disabledStar;
                if (wordPackClone.GetComponent<IAPButton>() != null)
                    StartCoroutine(disableIAPButton(wordPackClone));
                break;
            case ProductState.unpurchased:
                var iAPButton = wordPackClone.GetComponent<IAPButton>();
                iAPButton.productId = wordPackData.wordPackProductIdentifier;
                iAPButton.UpdateText();
                star.sprite = lockImage;
                break;
            case ProductState.unavailable:
                star.sprite = lockImage;
                break;
        }
    }

    IEnumerator disableIAPButton(GameObject wordPackClone)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(wordPackClone.GetComponent<IAPButton>());   
    }

    public void populateSelectedWordPacks()
    {
        selectedWordPacks.Clear();
        foreach (WordPackProduct product in wordPacksToSelect)
        {
            if (product.state == ProductState.enabled)
            {
                selectedWordPacks.Add(product);
            }
        }

        var gameCreation = gameObject.GetComponentInChildren<GameCreationScript>();
        gameCreation.wordPacksToUse = selectedWordPacks;
        gameCreation.checkIfAtLeastOneWordPack();
    }

    public void runMainBoardAnimation()
    {
        var mainBoardAnimator =  GameObject.Find("MainBoardCanvas").GetComponent<Animator>();
        mainBoardAnimator.Play("MainBoardAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
