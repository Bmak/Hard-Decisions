using System;
using UnityEngine;

public abstract class SolitareObject : MonoBehaviour
{
    public abstract void SetPhase(SolGamePhase phase);
    public abstract bool IsComplete { get; set; }
    public Action OnComplete = () => { };
}
