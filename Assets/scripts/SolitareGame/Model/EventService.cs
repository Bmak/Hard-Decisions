
using System.Collections.Generic;
using UnityEngine;

public class EventService : MonoBehaviour
{
    [SerializeField] public List<EventDef> Events = new List<EventDef>();

    private void Awake()
    {
        S.EventService = this;
    }

    public EventDef GetRandomEvent()
    {
        return Events[Random.Range(0, Events.Count)];
    }

    public void AcceptEvent(EventDef def)
    {
        foreach (FacilityEventBuff buff in def.Buffs)
        {
            buff.Buffs.ForEach(b => S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById(buff.Def.Id), "event" + def.Id, b));
        }
    }
}
