using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotItemView : SlotObject
{
    [SerializeField] private Image _bkg;
    [SerializeField] private Text _title;
    [SerializeField] private Text _amountText;
    [SerializeField] private ProfStatView _profStatPrefab;
    [SerializeField] private HorizontalLayoutGroup _cardHolder;
    [SerializeField] private Transform _statsContainer;

    private List<CharObject> _cards = new List<CharObject>();
    private FacilityDef _def;
    private FacilityDef _originalDef;

    private List<ProfStatView> _stats = new List<ProfStatView>();

    private DropHandler _dropHandler;

    private int _slotHasCharge;

    private void Awake()
    {
        _dropHandler = GetComponentInChildren<DropHandler>();
        _dropHandler.OnDropCard += OnDropCard;
    }

    public void Init(FacilityDef facility)
    {
        _def = facility;
        _originalDef = facility.DeepCopy();
        _title.text = _def.Name;

        S.FacilityService.OnFacilityUpdate += OnUpdateSlot;

        RedrawSlot();
    }

    private void OnUpdateSlot(SlotObject slot)
    {
        if (slot != this) return;

        //TODO too heavy
        _def = _originalDef.DeepCopy();

        foreach (KeyValuePair<string, FacilityBuff> pair in slot.Buffs)
        {
            var buff = pair.Value;

            _def.Charge += buff.Charge;
        }

        RedrawSlot();
    }

    private void RedrawSlot()
    {
        _stats.ForEach(x => Destroy(x.gameObject));
        _stats.Clear();

        _profStatPrefab.gameObject.SetActive(true);
        foreach (ProfessionType stat in _def.ProfessionTypes)
        {
            ProfStatView statView = Instantiate(_profStatPrefab, _statsContainer);
            statView.SetStat(stat, 0);
            _stats.Add(statView);
        }
        _profStatPrefab.gameObject.SetActive(false);
        UpdateAmount();
    }

    private void UpdateAmount()
    {
        _slotHasCharge = 0;
        int youNeed = _def.Charge;

        foreach (CharObject charObject in Cards)
        {
            var filtered = charObject.GetCurrentSlots().Where(s => _def.ProfessionTypes.Contains(s.Type)).ToList();
            filtered.ForEach(x => _slotHasCharge += x.Amount);
            /*
            var amounts = _def.ProfessionTypes.Join(charObject.Def.Stats, prof => prof, charProf => charProf.Type,
                (prof, charProf) => new { T = charProf.Type, Amount = charProf.Amount }).ToList();

            amounts.ForEach(x =>
            {
                if (x.T != ProfessionType.General)
                    youHave += x.Amount;
            });

            if (charObject.Def.Stats.Exists(x => x.Type == ProfessionType.General))
            {
                youHave += charObject.Def.Stats.FirstOrDefault(x => x.Type == ProfessionType.General).Amount;
            }
            */
        }

        _amountText.text = string.Format("{0}/{1}", _slotHasCharge, youNeed);
    }

    public override void SetPhase(SolGamePhase phase)
    {
        if (phase == SolGamePhase.StartReset)
        {
            Buffs.Clear();
            _originalDef.Charge = S.FacilityService.GetChargeById(this);
            Debug.Log(string.Format("Slot {0} -> Day Charge: {1}", Def.Name, _originalDef.Charge));
            OnUpdateSlot(this);
        }

        if (phase == SolGamePhase.Night)
        {
            int delta = _slotHasCharge - _def.Charge;
            S.VitalService.ChangeVitalAfterNight(_def.Vital, delta);
        }

        IsComplete = true;
        OnComplete();
    }

    public override bool IsComplete { get; set; }

    public override List<CharObject> Cards
    {
        get { return _cards; }
    }

    public override FacilityDef Def
    {
        get { return _def; }
        set { _def = value; }
    }

    public override Action<Action> GetAnim(string buffKey, bool remove = false)
    {
        return c =>
        {
            if (!Buffs.ContainsKey(buffKey))
            {
                Debug.LogError(string.Format("Buff with key: {} doesn't contains key, this error for animations", buffKey));
            }
            FacilityBuff buff = Buffs[buffKey];
            if (remove)
            {
                Buffs.Remove(buffKey);
                buff.Charge = -buff.Charge;
            }
            
            TextCountAnim.CreateTextAnim(_amountText, _def.Charge, _def.Charge + buff.Charge, c);
            //_def.Charge += buff.Charge;
            OnUpdateSlot(this);
            //UpdateAmount();
        };
    }

    private void OnDropCard()
    {
        return;
        for (int i = 0; i < _cardHolder.transform.childCount; i++)
        {
            Transform child = _cardHolder.transform.GetChild(i);
            CharObject charObject = child.GetComponent<CharObject>();
            if (_cards.Contains(charObject)) continue;

            _cards.Add(charObject);
            charObject.Slot = this;

            charObject.SetPhase(SolGamePhase.Setup);

            break;
        }
    }

    //TODO this things really heavy
    private int _childCount = 0;
    private void Update()
    {
        if (_childCount != _cardHolder.transform.childCount)
        {
            _childCount = _cardHolder.transform.childCount;
            OnChildCountChange();
        }
    }

    private void OnChildCountChange()
    {
        //todo update view
        //todo set card effect

        _cards.Clear();
        for (int i = 0; i < _cardHolder.transform.childCount; i++)
        {
            _cards.Add(_cardHolder.transform.GetChild(i).GetComponent<CharObject>());
        }

        RedrawSlot();
    }
    

}
