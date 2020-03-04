using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler {

    public Image bg = null;
    Color bgStartColor;
    bool bgStartIsEnabled;

    public int dropZoneCapacity = -1; // -1 is no cap; 0 and above is the cap


    void Start() {
        if (bg == null) {
            bg = gameObject.GetComponent<Image>();
        }

        bgStartColor = bg.color;
        bgStartIsEnabled = bg.enabled;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(name + ": OnPointerEnter");
        if (eventData.pointerDrag == null) {
            //print(name + ": eventData.pointerDrag == null");
            return;
        }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            if (dropZoneCapacity == -1 || dropZoneCapacity > this.transform.childCount) {
                // d.placeholderParent = this.transform;

                bg.enabled = true;
                bg.color = Color.white;
            } else {
                print(name + ": dropZoneCapacity is off somehow");
            }
        } else {
            print(name + ": d == null");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(name + ": OnPointerExit");
        if (eventData.pointerDrag == null) {
            //print(name + ": eventData.pointerDrag == null");
            return;
        }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null /* &&  d.placeholderParent == this.transform */)
        {
            // d.placeholderParent = d.landZone;

            bg.color = bgStartColor;
            bg.enabled = bgStartIsEnabled;
        } else {
            print(name + ": d == null ?");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            if (dropZoneCapacity == -1 || dropZoneCapacity > this.transform.childCount) {
                d.landZone = this.transform;

                bg.color = bgStartColor;
                bg.enabled = bgStartIsEnabled;

                FindObjectOfType<GameCtrl>().IsNewDayReady();

            } else {
                print(name + ": dropZoneCapacity is off somehow");
            }
        } else {
            print(name + ": d == null");
        }
    }

}