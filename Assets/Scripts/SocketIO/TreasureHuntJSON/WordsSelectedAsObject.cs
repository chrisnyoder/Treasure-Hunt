using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class WordsSelectedAsObject
{
    public List<string> listOfWordsSelected = new List<string>(){};
    public string lastWordSelected { 
        get 
        {
            return listOfWordsSelected.LastOrDefault();
        }
    }
}

