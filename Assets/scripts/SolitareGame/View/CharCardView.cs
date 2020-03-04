using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DragHandler))]
public class CharCardView : CharObject
{
    [SerializeField] private Transform _container;
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Image _icon;
    [SerializeField] private Transform _professions;
    [SerializeField] private ProfStatView _profStatPrefab;
    [SerializeField] private EffectCardView _effectCardView;

    private CharacterScrObj _def;

    private List<CardScrObj> _deck = new List<CardScrObj>();
    private List<CardScrObj> _drop = new List<CardScrObj>();

    private List<ProfStatView> _profStats = new List<ProfStatView>();

    private DragHandler _dragHandler;

    private void Awake()
    {
        _dragHandler = GetComponent<DragHandler>();
        _dragHandler.OnStartDragCard += OnStartDragCard;
        _dragHandler.OnEndDragCard += OnEndDragCard;
    }

    public void SetData(CharacterScrObj def)
    {
        _def = def;

        _title.text = def.firstName + " " + def.lastName;
        _description.text = def.description;
        _icon.sprite = def.artwork;

        S.CharacterService.OnUpdateChar += OnUpdateChar;

        _def.cardsTraits.ForEach(c => _deck.Add(c.DeepCopy()));

        OnUpdateChar(this);

        _effectCardView.gameObject.SetActive(false);
    }

    private void OnUpdateChar(CharObject obj)
    {
        if (obj != this) return;

        _def.Stats.Clear();
        var original = S.CharacterService.CharactersDict[_def.firstName + _def.lastName];
        original.Stats.ForEach(x => _def.Stats.Add(x.DeepCopy()));
        

        foreach (KeyValuePair<string, CharBuff> pair in obj.Buffs)
        {
            CharBuff buff = pair.Value;

            foreach (ProfessionStat stat in buff.Stats)
            {
                var s = _def.Stats.Find(x => x.Type == stat.Type);
                if (s == null)
                {
                    _def.Stats.Add(stat);
                }
                else
                    s.Amount += stat.Amount;
            }
        }
        
        RedrawCharCard();
    }

    private void RedrawCharCard()
    {
        _profStats.ForEach(p => Destroy(p.gameObject));
        _profStats.Clear();

        _profStatPrefab.gameObject.SetActive(true);
        foreach (ProfessionStat professionStat in _def.Stats)
        {
            if (professionStat.Amount == 0) continue;
            ProfStatView statView = Instantiate(_profStatPrefab, _professions);
            statView.SetStat(professionStat.Type, professionStat.Amount);
            _profStats.Add(statView);
        }
        _profStatPrefab.gameObject.SetActive(false);
    }

    public override void SetPhase(SolGamePhase phase)
    {
        if (phase == SolGamePhase.StartReset)
        {
            _effectCardView.gameObject.SetActive(false);
            _effectCardView.SetPhase(phase);
            OnFullComplete();
            DropCard();
            return;
        }

        if (phase == SolGamePhase.Draw)
        {
            _effectCardView.SetData(DrawCard());
            _effectCardView.gameObject.SetActive(true);
            _effectCardView.Char = this;
            _effectCardView.transform.SetAsLastSibling();
            _effectCardView.SetPhase(phase);
            _effectCardView.transform.DOShakeRotation(1).OnComplete(() =>
            {
                _effectCardView.transform.SetAsFirstSibling();
                OnFullComplete();
            });
            
            return;
        }

        _effectCardView.SetPhase(phase);
        OnFullComplete();
    }

    private void OnStartDragCard()
    {
        if (Slot != null && _effectCardView.Def != null)
        {
            Slot = null;
            _effectCardView.RevertEffect();
        }
    }

    private void OnEndDragCard()
    {
        if (Slot == null)
        {
            Slot = transform.parent.GetComponentInParent<SlotObject>();
            SetPhase(SolGamePhase.Setup);
        }
    }

    private void OnFullComplete()
    {
        IsComplete = true;
        OnComplete();
    }

    public override bool IsComplete { get; set; }

    private CardScrObj DrawCard()
    {
        if (_deck.Count == 0)
        {
            var shuffle = ProBro.ShuffleList(_drop);
            _deck.AddRange(shuffle);
            _drop.Clear();
        }

        int rnd = UnityEngine.Random.Range(0, _deck.Count);
        CardScrObj card = _deck[rnd];
        _deck.RemoveAt(rnd);
        return card;
    }

    private void DropCard()
    {
        Slot = null;
        if (_effectCardView.Def != null)
        {
            _drop.Add(_effectCardView.Def);
            _effectCardView.Def = null;
        }
    }

    public override CharacterScrObj Def
    {
        get { return _def; }
        set { _def = value; }
    }

    public override List<CardScrObj> Drop
    {
        get { return _drop; }
    }

    public override List<CardScrObj> Deck
    {
        get { return _deck; }
    }

    public override SlotObject Slot { get; set; }
    public override Action<Action> GetAnim(string buffKey, bool remove = false)
    {
        return callback =>
        {
            CharBuff buff = Buffs[buffKey];

            if (remove)
            {
                Buffs.Remove(buffKey);
            }

            TextCountAnim.CreateStatAnim(_profStats, callback);
            OnUpdateChar(this);
        };
    }
}
