﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public enum ProductState
{
    unpurchased,
    enabled,
    disabled, 
    unavailable
}

public struct WordPackProductIdentifiers 
{
    public static List<string> freeProductIdentifiers = new List<string>(){"initialWordList", "initialWordListJP"};
    public static List<string> productsForPurchaseIdentifers = new List<string>(){"initialWordListExpansion", "fantasyWordList", "celebritiesWordList", "initialWordListExpansionJP"};

    public static List<string> returnAllProucts()
    {
        var allProducts = freeProductIdentifiers.Concat(productsForPurchaseIdentifers).ToList();
        return allProducts;
    }
}

public class WordPackProduct
{
    public string wordPackProductIdentifier;    
    public string wordPackProductTitle;
    public string wordPackDescription; 
    public Sprite wordPackImage;
    public Language.Name language; 
    public bool isStarter;
    public string price; 
    public ProductState state;

    public WordPackProduct(string wordPackIdentifier)
    {
        this.wordPackProductIdentifier = wordPackIdentifier;
        this.state = (ProductState) Enum.Parse(typeof(ProductState), PlayerPrefs.GetString(wordPackIdentifier));
        
        switch (wordPackIdentifier)
        {
            case "initialWordList":
                this.wordPackProductTitle = LocalizationManager.instance.GetLocalizedText("starter_title");
                this.wordPackDescription = LocalizationManager.instance.GetLocalizedText("starter_description");
                this.language = Language.Name.English;
                this.isStarter = true;
                this.wordPackImage = Resources.Load<Sprite>("Images/ImagesWithText/" + LocalizationManager.instance.GetLocalizedText("starter_word_pack"));
                break;
            case "initialWordListExpansion":
                this.wordPackProductTitle = LocalizationManager.instance.GetLocalizedText("expansion_title");
                this.wordPackDescription = LocalizationManager.instance.GetLocalizedText("expansion_description");
                this.language = Language.Name.English;
                this.isStarter = false;
                this.wordPackImage = Resources.Load<Sprite>("Images/ImagesWithText/" + LocalizationManager.instance.GetLocalizedText("expansion_word_pack"));
                break;
            case "fantasyWordList":
                this.wordPackProductTitle = LocalizationManager.instance.GetLocalizedText("sci-fi__fantasy_title");
                this.wordPackDescription = LocalizationManager.instance.GetLocalizedText("sci-fi__fantasy_description");
                this.language = Language.Name.English;
                this.isStarter = false;
                this.wordPackImage = Resources.Load<Sprite>("Images/ImagesWithText/" + LocalizationManager.instance.GetLocalizedText("sci-fi_fantasy_word_pack"));
                break; 
            case "celebritiesWordList":
                this.wordPackProductTitle = LocalizationManager.instance.GetLocalizedText("celebrity_title");
                this.wordPackDescription = LocalizationManager.instance.GetLocalizedText("celebrity_description");
                this.language = Language.Name.English;
                this.wordPackImage = Resources.Load<Sprite>("Images/ImagesWithText/" + LocalizationManager.instance.GetLocalizedText("celebrity_word_pack"));
                this.isStarter = false;
                break; 
            case "initialWordListJP":
                this.wordPackProductTitle = LocalizationManager.instance.GetLocalizedText("starter_title");
                this.wordPackDescription = LocalizationManager.instance.GetLocalizedText("starter_description"); 
                this.language = Language.Name.Japanese;
                this.wordPackImage = Resources.Load<Sprite>("Images/ImagesWithText/" + LocalizationManager.instance.GetLocalizedText("starter_word_pack"));
                this.isStarter = true; 
                break;
            case "initialWordListExpansionJP":
                this.wordPackProductTitle = LocalizationManager.instance.GetLocalizedText("expansion_title");
                this.wordPackDescription = LocalizationManager.instance.GetLocalizedText("expansion_description");
                this.language = Language.Name.Japanese;
                this.wordPackImage = Resources.Load<Sprite>("Images/ImagesWithText/" + LocalizationManager.instance.GetLocalizedText("expansion_word_pack"));
                this.isStarter = false;
                break; 
        }
    }
}
