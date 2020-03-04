
using System.Collections.Generic;

public abstract class CardObject : SolitareObject
{
    public abstract CardScrObj Def { get; set; }
    public abstract CharObject Char { get; set; }
    public Dictionary<string, CardBuff> Buffs = new Dictionary<string, CardBuff>();
}

public class CardBuff
{
    public CardScrObj Def { get; set; }
}
