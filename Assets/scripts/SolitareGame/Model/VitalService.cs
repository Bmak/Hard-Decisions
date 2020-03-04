using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VitalService : MonoBehaviour
{
    public List<VitalStat> Stats = new List<VitalStat>();

    public Action OnVitalUpdated = () => { };

    private void Awake()
    {
        S.VitalService = this;
    }

    public VitalStat GetVital(VitalType type)
    {
        return Stats.FirstOrDefault(v => v.VType == type);
    }

    public void AddBuff(VitalType type, string key, int buff)
    {
        VitalStat vital = Stats.FirstOrDefault(v => v.VType == type);
        if (!vital.Buffs.ContainsKey(key))
        {
            vital.Buffs[key] = 0;
        }
        vital.Buffs[key] += buff;

        S.AnimManager.AddAnim(vital.GetAnim(buff));

        //CheckVital(vital);

        LogBuff(type, buff);
    }

    public void RemoveBuff(VitalType type, string key)
    {
        VitalStat vital = Stats.FirstOrDefault(v => v.VType == type);

        if (vital.Buffs.ContainsKey(key))
        {
            LogBuff(type, vital.Buffs[key], true);

            
            int buff = vital.Buffs[key];
            vital.Buffs.Remove(key);
            S.AnimManager.AddAnim(vital.GetAnim(-buff));
            return;
        }

        vital.Buffs.Remove(key);

        CheckVital(vital);
    }

    private void LogBuff(VitalType type, int amount, bool remove = false)
    {
        Debug.Log(string.Format("VitalType {0} -> {1} AmountBuff: {2}", type, remove ? "Remove" : "Add", amount));
    }

    public void ChangeVitalAfterNight(VitalType type, int delta)
    {
        VitalStat stat = GetVital(type);
        int old = stat.GetCurrentAmount();
        Debug.Log(string.Format("After Night Vital {0} Changed from: {1} to: {2}", stat.VType, old, old + delta));
        if (delta == 0) return;
        S.AnimManager.AddAnim(stat.GetAnim(delta));
        //stat.Amount += delta;
        //CheckVital(stat);
    }

    public void CheckGameOver()
    {
        if (GetVital(VitalType.HP).GetCurrentAmount() > 0) return;

        MessageDef def = new MessageDef()
        {
            Title = "GAME OVER",
            Caption = "Your base HP is 0!\nGame Over!\nTry again!",
            ButtonAction = RestartGame
        };

        Gui.ShowScreen<MessageScreen>().SetData(def);
    }

    public void CheckVital(VitalStat stat)
    {
        switch (stat.VType)
        {
            case VitalType.HP:
                stat.Amount = Math.Min(stat.Max, stat.Amount);
                CheckGameOver();
                break;
            case VitalType.FD:
                if (stat.GetCurrentAmount() < 0)
                {
                    ChangeVitalAfterNight(VitalType.HP, stat.Amount);
                    stat.Amount = 0;
                    stat.Buffs.Clear();
                }
                break;
            case VitalType.DEF:
                if (stat.GetCurrentAmount() < 0)
                {
                    ChangeVitalAfterNight(VitalType.HP, stat.Amount);
                    stat.Amount = 0;
                    stat.Buffs.Clear();
                }
                break;
            case VitalType.MOR:
                if (stat.GetCurrentAmount() <= 0)
                {
                    ChangeVitalAfterNight(VitalType.HP, Math.Min(-1, stat.GetCurrentAmount()));
                    stat.Amount = 1;
                    stat.Buffs.Clear();
                }
                break;
        }

        OnVitalUpdated();
    }

    public void RestartGame()
    {
        Debug.LogError("Not Implemented yet! Restart App!");
    }
}

[Serializable]
public class VitalStat
{
    public VitalType VType;
    public int Amount = 10;
    public int Start = 10;
    public int Max = 10;
    public Dictionary<string, int> Buffs = new Dictionary<string, int>();
    public Text Text;

    public int GetCurrentAmount()
    {
        int buff = 0;
        foreach (KeyValuePair<string, int> pair in Buffs)
        {
            buff += pair.Value;
        }
        return Amount + buff;
    }

    public Action<Action> GetAnim(int buff)
    {
        return callBack =>
        {
            TextCountAnim.CreateTextAnim(Text, Amount, Amount + buff, callBack, TextCountAnim.Direction.Down, true);
            S.VitalService.CheckVital(this);
        };
    }
}

public enum VitalType
{
    NONE,
    HP,
    FD,
    DEF,
    MOR,
    AP
}

public enum ProfessionType
{
    None,
    Medical,
    Aggressive,
    Engineering,
    Social,
    General
}
