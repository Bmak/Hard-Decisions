using System;
using System.Collections.Generic;
using DG.Tweening;
using MyExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectCardView : CardObject, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Image _icon;

    private CardScrObj _def;
    private SolGamePhase _currentPhase;

    private void Awake()
    {
        S.CharacterService.OnUpdateCard += OnUpdateCard;
    }

    public void SetData(CardScrObj def)
    {
        _def = def;
        RedrawCard();
    }

    public override void SetPhase(SolGamePhase phase)
    {
        _currentPhase = phase;
        if (_def == null)
        {
            OnComplete();
            return;
        }

        BaseEffect effect = EffectsManager.GetEffect(_def.name);
        if (effect == null)
        {
            OnComplete();
            return;
        }
        effect.OnComplete = OnComplete;
        effect.SetEffect(phase, this);

        S.AnimManager.SetPhase(_currentPhase);
    }

    public override bool IsComplete { get; set; }

    private Vector3 _pos;
    private DragZone _dragZone;
    private Transform _parent;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_dragZone == null)
        {
            _dragZone = FindObjectOfType<DragZone>();
        }
        _parent = transform.parent;
        transform.SetParent(_dragZone.transform);
        transform.SetAsLastSibling();
        //transform.localScale = Vector3.one * 3;
        _pos = transform.localPosition;
        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSize(rect.GetSize()*2);


        float cWidth = 0;
        float cHeight = rect.GetSize().y * 0.3f;
        Vector2 offset = new Vector2(cWidth, cHeight);

        Vector3 newPos = eventData.position + offset;
        transform.position = newPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //transform.localScale = Vector3.one;

        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSize(rect.GetSize() / 2);

        transform.localPosition = _pos;
        transform.SetParent(_parent);
        transform.SetAsFirstSibling();
    }

    public override CardScrObj Def
    {
        get { return _def; }
        set { _def = value; }
    }

    public override CharObject Char { get; set; }

    private void OnUpdateCard(CardObject obj)
    {
        if (obj != this) return;

        _def = S.CharacterService.EffectCardsDict[_def.name];

        bool flag = false;
        foreach (KeyValuePair<string, CardBuff> pair in obj.Buffs)
        {
            CardBuff buff = pair.Value;

            _def = buff.Def;

            flag = true;
        }
        if (flag)
        {
            transform.DORotate(new Vector3(0, 0, 360), 0.5f).OnComplete(() =>
            {
                //SetPhase(_currentPhase);
                RedrawCard();
            });
        }
        else
        {
            RedrawCard();
        }
        
    }

    private void RedrawCard()
    {
        _title.text = _def.name;
        _description.text = _def.description;
        _icon.sprite = _def.artwork;
    }

    public void RevertEffect()
    {
        BaseEffect effect = EffectsManager.GetEffect(_def.name);
        if (effect == null)
        {
            OnComplete();
            return;
        }
        effect.OnComplete = OnComplete;
        effect.RevertEffect();
        S.AnimManager.SetPhase(_currentPhase);
    }
}
