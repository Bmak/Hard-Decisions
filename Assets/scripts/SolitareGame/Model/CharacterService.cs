
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterService : MonoBehaviour
{
    public List<CharacterScrObj> CharacterDefs = new List<CharacterScrObj>();
    public List<CardScrObj> EffectCardDefs = new List<CardScrObj>();
    public Dictionary<string, CardScrObj> EffectCardsDict { get; set; }
    public Dictionary<string, CharacterScrObj> CharactersDict { get; set; }
    public List<CharCardView> Chars { get; set; }

    public Action<CharObject> OnUpdateChar = ch => { };
    public Action<CardObject> OnUpdateCard = card => { };

    private void Awake()
    {
        S.CharacterService = this;

        EffectCardsDict = new Dictionary<string, CardScrObj>();
        foreach (CardScrObj effectCard in EffectCardDefs)
        {
            EffectCardsDict[effectCard.name] = effectCard;
        }

        CharactersDict = new Dictionary<string, CharacterScrObj>();
        foreach (CharacterScrObj charCard in CharacterDefs)
        {
            CharactersDict[charCard.firstName+charCard.lastName] = charCard;
        }
    }

    public void AddCharBuff(CharObject charObject, string key, CharBuff buff)
    {
        if (charObject.Buffs.ContainsKey(key))
        {
            charObject.Buffs[key].ConcantenateBuff(buff);
        }
        else
        {
            charObject.Buffs[key] = buff;
        }

        S.AnimManager.AddAnim(charObject.GetAnim(key));

        //OnUpdateChar(charObject);

        LogCharBuff(charObject, buff);
    }

    public void RemoveCharBuff(CharObject charObject, string key)
    {
        if (charObject.Buffs.ContainsKey(key))
        {
            LogCharBuff(charObject, charObject.Buffs[key], true);

            S.AnimManager.AddAnim(charObject.GetAnim(key, true));
        }

        //charObject.Buffs.Remove(key);

        //OnUpdateChar(charObject);
    }

    public void AddCardBuff(CardObject card, string key, CardBuff buff)
    {
        card.Buffs[key] = buff;

        OnUpdateCard(card);

        LogCardBuff(card, buff);
    }

    public void RemoveCardBuff(CardObject card, string key)
    {
        if (card.Buffs.ContainsKey(key))
        {
            LogCardBuff(card, card.Buffs[key], true);
        }

        card.Buffs.Remove(key);

        OnUpdateCard(card);
    }

    private void LogCharBuff(CharObject charObject, CharBuff buff, bool remove = false)
    {
        Debug.Log(string.Format("CharObject: {0} -> {1} Stat Buff: {2}", charObject.Def.firstName, remove ? "Remove" : "Add", buff));
    }

    private void LogCardBuff(CardObject card, CardBuff buff, bool remove = false)
    {
        Debug.Log(string.Format("CharObject: {0} -> {1} Def Buff: {2}", card.Def.name, remove ? "Remove" : "Add", buff.Def.name));
    }

}
