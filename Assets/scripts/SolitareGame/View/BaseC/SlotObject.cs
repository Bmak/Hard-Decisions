using System;
using System.Collections.Generic;

public abstract class SlotObject : SolitareObject
{
    public abstract List<CharObject> Cards { get; }
    public abstract FacilityDef Def { get; set; }
    public Dictionary<string, FacilityBuff> Buffs  = new Dictionary<string, FacilityBuff>();

    public abstract Action<Action> GetAnim(string buffKey, bool remove = false);
}

[Serializable]
public class FacilityBuff
{
    public int Charge;
}