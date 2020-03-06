using System.Collections.Generic;
using UnityEngine;

public sealed class Language
{
    public enum Name
    {
        English, 
        Japanese
    }

    public static string getLanguageCode(SystemLanguage language)
    {
        string code = "en";

        switch(language)
        {
            case SystemLanguage.English:
                code = "en";
                break;
            case SystemLanguage.Japanese:
                code = "jp";
                break;
        }
        return code;
    }
}

