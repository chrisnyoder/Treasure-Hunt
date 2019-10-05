using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public enum ProductLanguage
{
    English, 
    Japanese
}

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
    public ProductLanguage language; 
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
                this.wordPackProductTitle = "Starter Word Pack";
                this.wordPackDescription = "The starter word pack provides the first set of 50 words you need to get playing. Example words: license, jail, reporter, umbrella, pinch, jet, rust.";
                this.language = ProductLanguage.English;
                this.isStarter = true;
                break;
            case "initialWordListExpansion":  
                this.wordPackProductTitle = "Expansion Word Pack";
                this.wordPackDescription = "The expansion pack gives more game to game variation with 100 more words similar to those in the starter pack. Example words: graduation, dress, New Year, lipstick, heart, goalie.";
                this.language = ProductLanguage.English;
                this.isStarter = false;
                break;
            case "fantasyWordList":
                this.wordPackProductTitle = "Sci Fi/Fantasy Word Pack";
                this.wordPackDescription = "The Sci Fi and Fantasy word pack has 50 words from across the most popular science fiction, fantasy, and comic book universes. Example words: Hal 9000, Alien, Batman, Hellboy, Gandalf.";
                this.language = ProductLanguage.English;
                this.isStarter = false;
                break; 
            case "celebritiesWordList":
                this.wordPackProductTitle = "Celebrity Word Pack";
                this.wordPackDescription = "The celebrity word pack consists of 50 names of the world's most popular celebrities. Example words: Kanye West, Eminem, Usain Bolt, Queen Elizabeth II, J.K. Rowling.";
                this.language = ProductLanguage.English;
                this.isStarter = false;
                break; 
            case "initialWordListJP":
                this.wordPackProductTitle = "Basic Japanese Word Pack";
                this.wordPackDescription = "The starter pack provides the first set of words you need to play. These ones are in Japanese.";
                this.language = ProductLanguage.Japanese;
                this.isStarter = true; 
                break;
            case "initialWordListExpansionJP":
                this.wordPackProductTitle = "Expansion Japanese Word Pack";
                this.wordPackDescription = "More game to game variation with 100 more Japanese words, similar to that of the Basic Word pack.";
                this.language = ProductLanguage.Japanese;
                this.isStarter = false;
                break; 
        }
    }
}
