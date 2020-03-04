using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    
    public List<CharController> unitsPool;
    public List<CharController> charactersOnHold;

    public Transform facilityHold;

    public void SendCharsToHold(List<CharController> charCards)
    {
        foreach (var cc in charCards)
        {
            charactersOnHold.Add(cc);
            unitsPool.Add(cc);
            cc.transform.SetParent(facilityHold);
            cc.SetDraggable(true);
        }
    }



}