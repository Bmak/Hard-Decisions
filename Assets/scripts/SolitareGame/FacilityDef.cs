
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
[CreateAssetMenu(fileName = "New Facility", menuName = "Facility", order = 0)]
public class FacilityDef : ScriptableObject
{
    public string Id;
    public string Name;
    public string Description;
    public List<ProfessionType> ProfessionTypes;
    public VitalType Vital;
    public int Charge;

    public FacilityDef DeepCopy()
    {
        FacilityDef def = Object.Instantiate(this);
        def.Id = this.Id;
        def.Name = this.Name;
        def.Description = this.Description;
        def.Vital = this.Vital;
        def.Charge = this.Charge;
        
        def.ProfessionTypes = new List<ProfessionType>();

        ProfessionTypes.ForEach(x => def.ProfessionTypes.Add(x));

        return def;
    }
}
