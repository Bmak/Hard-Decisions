using MyExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private DragScaler _dragScaler;
    private DragZone _dragZone;
    public Transform ParrentTransform;
    public Transform LandZone;

    private Vector2 _offset;
    private CanvasGroup _canvasGroup;

    public Action OnStartDragCard = () => { };
    public Action OnEndDragCard = () => { };

    void Start()
    {
        _dragScaler = gameObject.GetComponentInChildren<DragScaler>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_dragZone == null)
        {
            _dragZone = FindObjectOfType<DragZone>();
        }

        float cWidth = GetComponent<RectTransform>().GetSize().x * 0.2f;
        float cHeight = GetComponent<RectTransform>().GetSize().y * 0.2f;
        _offset = new Vector2(cWidth, -cHeight);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_dragScaler != null)
        {
            _dragScaler.transform.localScale = Vector3.one * _dragScaler.SCALE;
        }

        ParrentTransform = gameObject.transform.parent;
        gameObject.transform.SetParent(_dragZone.transform);
        LandZone = ParrentTransform;

        _canvasGroup.blocksRaycasts = false;

        OnStartDragCard();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragScaler != null)
        {
            _dragScaler.transform.localScale = Vector3.one;
        }
        /*
        foreach (GameObject o in eventData.hovered)
        {
            DropHandler dh = o.GetComponent<DropHandler>();
            if (dh != null && dh.transform != ParrentTransform)
            {
                gameObject.transform.SetParent(dh.transform);
                dh.OnDrop();
                return;
            }
        }
        */
        gameObject.transform.SetParent(LandZone);

        _canvasGroup.blocksRaycasts = true;

        OnEndDragCard();
    }
}
