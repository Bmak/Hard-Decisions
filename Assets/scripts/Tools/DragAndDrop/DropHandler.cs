using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public Action OnDropCard = () => { };
    public int Capacity = 1;

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount >= Capacity) return;

        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();
        if (d != null)
        {
            if (d.ParrentTransform != transform)
            {
                OnDropCard();
            }

            d.LandZone = transform;
        }
    }
}
