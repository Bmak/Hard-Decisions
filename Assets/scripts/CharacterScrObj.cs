using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Characer", menuName = "Character", order = 0)]
public class CharacterScrObj : ScriptableObject {
    
    public string firstName;
    public string lastName;
    public string description;
    public bool infected;
    public Sprite artwork;

    public List<ProfessionStat> Stats = new List<ProfessionStat>();

    // 0 - state card, determined by gen, not random
    // 1 - backstory card, randoms only with other backs
    // 2, 3, 4 - trait cards, can be random
    public List<CardScrObj> cardsTraits = new List<CardScrObj>();

    // 3 loot cards, can be random
    public List<CardScrObj> cardsLoot = new List<CardScrObj>();

    public CharacterScrObj DeepCopy() {
        CharacterScrObj obj = new CharacterScrObj();

        obj.firstName = firstName;
        obj.lastName = lastName;
        obj.description = description;
        obj.infected = infected;
        obj.artwork = artwork;

        obj.cardsTraits = new List<CardScrObj>();
        for (int i = 0; i < this.cardsTraits.Count; ++i)
        {
            obj.cardsTraits.Add(this.cardsTraits[i].DeepCopy());
        }
        obj.cardsLoot = new List<CardScrObj>();
        for (int i = 0; i < this.cardsLoot.Count; ++i)
        {
            obj.cardsLoot.Add(this.cardsLoot[i].DeepCopy());
        }
        obj.Stats = new List<ProfessionStat>();

        Stats.ForEach(x => obj.Stats.Add(x.DeepCopy()));

        return obj;
    }

}

[Serializable]
public class ProfessionStat
{
    public ProfessionType Type;
    public int Amount;

    public ProfessionStat DeepCopy()
    {
        var s = new ProfessionStat();
        s.Type = this.Type;
        s.Amount = this.Amount;
        return s;
    }

    public static ProfessionType GetRandomType(params ProfessionType[] exclusive)
    {
        Array types = Enum.GetValues(typeof(ProfessionType));
        
        ProfessionType type;
        do
        {
            int id = Random.Range(0, types.Length);
            type = (ProfessionType)types.GetValue(id);
        } while (!exclusive.Contains(type));

        return type;
    }
}