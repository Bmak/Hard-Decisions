using System;
using System.Collections.Generic;

public abstract class CharObject : SolitareObject
{
    public abstract CharacterScrObj Def { get; set; }
    public abstract List<CardScrObj> Drop { get; }
    public abstract List<CardScrObj> Deck { get; }
    public abstract SlotObject Slot { get; set; }
    public Dictionary<string, CharBuff> Buffs = new Dictionary<string, CharBuff>();

    public abstract Action<Action> GetAnim(string buffKey, bool remove = false);

    public List<ProfessionStat> GetCurrentSlots()
    {
        var result = new List<ProfessionStat>();
        result.AddRange(Def.Stats);
        foreach (var buff in Buffs)
        {
            result.AddRange(buff.Value.Stats);
        }
        return result;
    }
}

public class CharBuff
{
    public List<ProfessionStat> Stats = new List<ProfessionStat>();

    public void AddCharStat(ProfessionType type, int amount)
    {
        ProfessionStat stat = new ProfessionStat();
        stat.Type = type;
        stat.Amount = amount;

        Stats.Add(stat);
    }

    public void ConcantenateBuff(CharBuff buff)
    {
        Stats.AddRange(buff.Stats);
    }

    public override string ToString()
    {
        string result = string.Empty;
        foreach (ProfessionStat stat in Stats)
        {
            result += string.Format("{0}: {1},", stat.Type, stat.Amount);
        }
        return result;
    }
}
