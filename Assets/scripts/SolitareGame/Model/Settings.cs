using System;
using System.Collections.Generic;
using UnityEngine;


public class Settings : MonoBehaviour
{
    [SerializeField] public List<ProfessionColor> Colors;

    void Awake()
    {
        S.Colors = Colors;
    }
}

[Serializable]
public class ProfessionColor
{
    public ProfessionType Type;
    public Color Color;
}

public static class S
{
    public static List<ProfessionColor> Colors { get; set; }

    public static VitalService VitalService { get; set; }
    public static CharacterService CharacterService { get; set; }
    public static FacilityService FacilityService { get; set; }
    public static EventService EventService { get; set; }
    public static AnimManager AnimManager { get; set; }


    public static Color GetColor(ProfessionType type)
    {
        return Colors.Find(x => x.Type == type).Color;
    }

}