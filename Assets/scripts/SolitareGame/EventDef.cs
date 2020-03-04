
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
[CreateAssetMenu(fileName = "New Event", menuName = "Event", order = 0)]
public class EventDef : ScriptableObject
{
    public string Id;
    public string Description;
    public List<FacilityEventBuff> Buffs = new List<FacilityEventBuff>();
}

[Serializable]
public class FacilityEventBuff
{
    public FacilityDef Def;
    public List<FacilityBuff> Buffs = new List<FacilityBuff>();
}

