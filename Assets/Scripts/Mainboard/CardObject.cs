
using System;

public enum CardType
{
    blueCard,
    redCard,
    neutralCard,
    shipwreckCard
}

[Serializable]
public class CardObject
{
    public CardType cardType;
    public string labelText;

    public CardObject(CardType cardType, string labelText)
    {
        this.cardType = cardType;
        this.labelText = labelText;
    }
}
