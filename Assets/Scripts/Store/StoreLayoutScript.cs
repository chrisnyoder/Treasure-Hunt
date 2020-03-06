using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using DG.Tweening;

public class StoreLayoutScript : MonoBehaviour
{
    public GameObject storeCollectionView;
    private RectTransform storeCollectionViewRT;
    public GameObject wordPackButton; 
    public SystemLanguage languageSelected; 
    public GameObject restorePurchaseButton;
    
    List<WordPackProduct> wordPacks = new List<WordPackProduct>(){};
    List<WordPackProduct> selectedWordPacks = new List<WordPackProduct>(){};
    List<Text> prices = new List<Text>{};

    public Sprite starterWordPackImage;
    public Sprite expansionWordPackImage;
    public Sprite fantasyWordPackImage;
    public Sprite celebrityWordPackImage;

    private bool pricesSet = false;
    private IAPButton iAPButton; 

    public Sprite enabledStar; 
    public Sprite disabledStar; 
    public Sprite lockImage; 

    private void Awake() 
    {
        languageSelected = LocalizationManager.instance.language;
    }
    
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
        wordPacks.Clear();
        var allWordPacks = WordPackProductIdentifiers.returnAllProucts();

        foreach(string wordpackidentifier in allWordPacks)
        {
            var wordPackObject = new WordPackProduct(wordpackidentifier);

            if(wordPackObject.language == languageSelected)
            {
                wordPacks.Add(wordPackObject);
            }
        }
        instantiateWordPackPrefabsAndPlaceImages();
    }

    public void instantiateWordPackPrefabsAndPlaceImages()
    {
        var wordPackPosition = new Vector3(0, 0, 0);
        var storeCollectionViewRT = storeCollectionView.GetComponent<RectTransform>();
        storeCollectionViewRT.sizeDelta = new Vector2(0, 0);

        var wordPackWidth = (storeCollectionViewRT.rect.width*0.75f)/4;
        var emptySpace = (storeCollectionViewRT.rect.width - (wordPackWidth*wordPacks.Count))/(wordPacks.Count+1);
        float xCardPosition = emptySpace + (wordPackWidth/2); 

        foreach(WordPackProduct wordPack in wordPacks)
        {
            GameObject wordPackClone = Instantiate(wordPackButton, wordPackPosition, Quaternion.identity, storeCollectionView.transform);

            Image wordPackCloneImage = wordPackClone.GetComponent<Image>();
            wordPackClone.GetComponent<StoreButtonHandler>().wordPackProduct = wordPack;
            wordPackClone.name = wordPack.wordPackProductIdentifier;

            wordPackCloneImage.sprite = wordPack.wordPackImage;

            var wordPackCloneRT = wordPackClone.GetComponent<RectTransform>();
            wordPackCloneRT.sizeDelta = new Vector2(wordPackWidth, wordPackWidth * 1.7f);
            wordPackCloneRT.anchoredPosition = new Vector3(xCardPosition, 50 , 0);

            xCardPosition += wordPackWidth+emptySpace; 
            displayProductState(wordPackClone);
        }    

        GameObject.Destroy(wordPackButton.gameObject, 0f);
        populateSelectedWordPacks();
    }

    public void displayProductState(GameObject wordPackClone)
    {

        Image star = wordPackClone.transform.GetChild(0).GetComponent<Image>();
        
        ParticleSystem particles = wordPackClone.transform.GetChild(0).GetComponentInChildren<ParticleSystem>();
        var em = particles.emission;
        
        var wordPackData = wordPackClone.GetComponent<StoreButtonHandler>().wordPackProduct;  
        Text price = wordPackClone.GetComponentInChildren<Text>();
        
        price.enabled = false;


        if (GlobalDefaults.Instance.tutorialIsOn && wordPackData.isStarter == true)
        {
            wordPackData.state = ProductState.enabled;
        }

        switch (wordPackData.state)
        {
            case ProductState.enabled:
                star.sprite = enabledStar;
                particles.Play();

                em.enabled = true;  
                
                if (wordPackClone.GetComponent<IAPButton>() != null)
                    StartCoroutine(disableIAPButton(wordPackClone));
                break;
            case ProductState.disabled:
                star.sprite = disabledStar;
                em.enabled = false;
                if (wordPackClone.GetComponent<IAPButton>() != null)
                    StartCoroutine(disableIAPButton(wordPackClone));
                break;
            case ProductState.unpurchased:
                var iAPButton = wordPackClone.GetComponent<IAPButton>();
                iAPButton.productId = wordPackData.wordPackProductIdentifier;
                em.enabled = false;

                if(price.text != "0" && price.text != "price")
                    price.enabled = true;
                star.sprite = lockImage;
                break;
            case ProductState.unavailable:
                star.sprite = lockImage;
                em.enabled = false;
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
        foreach (WordPackProduct product in wordPacks)
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

    // Update is called once per frame
    void Update()
    {
        if(!pricesSet)
        {

        }
    }
}
