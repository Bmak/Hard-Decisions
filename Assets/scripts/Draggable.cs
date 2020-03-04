using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyExtensions;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    Vector2 offset;
    public Transform preDragZone;
    public Transform dragZone;
    public Transform landZone;

    // public Transform placeholderParent = null;
    // GameObject placeholder = null;


    void Start() {
        float cWidth = GetComponent<RectTransform>().GetSize().x * 0.6f; 
        float cHeight = GetComponent<RectTransform>().GetSize().y * 0.6f;
        offset = new Vector2(cWidth, -cHeight);

        //if (dragZone == null) {
            dragZone = GameObject.Find("canvas_overlay").transform;
        //}
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        print(">>>>>>>>>>>> BEGIN drag");

        // // make placeholder
        // placeholder = new GameObject();
        // placeholder.transform.SetParent(this.transform.parent);
        // LayoutElement le = placeholder.AddComponent<LayoutElement>();
        // le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        // le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        // le.flexibleWidth = 0;
        // le.flexibleHeight = 0;

        // placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //set offset & zones; unblock raycasts
        //offset = (Vector2)transform.localPosition - eventData.position;

        preDragZone = transform.parent;
        transform.SetParent(dragZone);
        landZone = preDragZone;
        // placeholderParent = landZone;


        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector3)(eventData.position + offset);

        // if (placeholder.transform.parent != placeholderParent)
        //     placeholder.transform.SetParent(placeholderParent);

        // int newSiblingIndex = placeholderParent.childCount;

        // for (int i = 0; i < placeholderParent.childCount; i++) {
        //     if (this.transform.position.x < placeholderParent.GetChild(i).position.x) {
        //         newSiblingIndex = i;

        //         if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
        //             newSiblingIndex--;

        //         break;
        //     }
        // }

        // placeholder.transform.SetSiblingIndex(newSiblingIndex);

    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        print("END drag >>>>>>>>>>>>>>");

        transform.SetParent(landZone);
        // this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Destroy(placeholder);
    }

}
