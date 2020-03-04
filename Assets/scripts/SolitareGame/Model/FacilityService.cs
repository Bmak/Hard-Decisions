using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FacilityService : MonoBehaviour
{
    public List<FacilityDef> FacilityDefs = new List<FacilityDef>();
    public List<SlotItemView> Slots { get; set; }

    public Action<SlotObject> OnFacilityUpdate = s => { };

    private void Awake()
    {
        S.FacilityService = this;
    }

    public SlotObject GetSlotById(string slotId)
    {
        return Slots.FirstOrDefault(s => s.Def.Id == slotId);
    }

    public void AddFacilityBuff(SlotObject slot, string key, FacilityBuff buff)
    {
        slot.Buffs[key] = buff;

        S.AnimManager.AddAnim(slot.GetAnim(key));

        //OnFacilityUpdate(slot);

        LogBuff(slot, buff);
    }

    public void RemoveFacilityBuff(SlotObject slot, string key)
    {
        if (slot.Buffs.ContainsKey(key))
        {
            LogBuff(slot, slot.Buffs[key], true);
            S.AnimManager.AddAnim(slot.GetAnim(key, true));
        }

        //slot.Buffs.Remove(key);

        //OnFacilityUpdate(slot);
    }

    private void LogBuff(SlotObject slot, FacilityBuff buff, bool remove = false)
    {
        Debug.Log(string.Format("Slot: {0} -> {1} Charge Buff: {2}", slot.Def.Id, remove ? "Remove" : "Add", buff.Charge));
    }

    public int GetChargeById(SlotObject slot)
    {
        switch (slot.Def.Id)
        {
            case "Kitchen":
                int kv = Mathf.RoundToInt(0.5f * S.CharacterService.Chars.Count);
                return kv;
            case "Perimeter":
                VitalStat defStat = S.VitalService.GetVital(VitalType.DEF);
                float pv = 0.5f * (defStat.Start - defStat.Amount);
                return pv < 0 ? 0 : (int)pv;
            case "Medical":
                VitalStat medStat = S.VitalService.GetVital(VitalType.HP);
                int mv = 0; //1 * (medStat.Start - medStat.Amount);
                return mv < 0 ? 0 : mv;
            case "Engineering":
                VitalStat dStat = S.VitalService.GetVital(VitalType.DEF);
                float ev = 0.5f * (dStat.Start - dStat.Amount);
                return ev < 0 ? 0 : (int)ev;
        }
        return 1;
    }

    public void NightReport(SlotObject slot)
    {
        
    }
}
