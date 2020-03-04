using System.Collections.Generic;
using UnityEngine;

public class SlotsController : SolitareObject
{
    [SerializeField] private SlotItemView _slotPrefab;
    [SerializeField] private Transform _slotsContainer;

    public List<SlotItemView> Slots = new List<SlotItemView>();

    private void Awake()
    {
        foreach (FacilityDef facility in S.FacilityService.FacilityDefs)
        {
            SlotItemView slot = Instantiate(_slotPrefab, _slotsContainer);
            slot.Init(facility.DeepCopy());
            Slots.Add(slot);
        }

        S.FacilityService.Slots = Slots;

        _slotPrefab.gameObject.SetActive(false);
    }

    public override void SetPhase(SolGamePhase phase)
    {
        if (phase == SolGamePhase.Report)
        {
            EventDef eventDef = S.EventService.GetRandomEvent();

            MessageDef mDef = new MessageDef()
            {
                Title = "Ночью что-то произошло!",
                Caption = eventDef.Description,
                ButtonAction = () => { OnAcceptNightEvent(eventDef); }
            };
            Gui.ShowScreen<MessageScreen>().SetData(mDef);
            return;
        }

        IsComplete = true;
        OnComplete();
    }

    private void OnAcceptNightEvent(EventDef def)
    {
        S.EventService.AcceptEvent(def);

        IsComplete = true;
        OnComplete();
    }

    public override bool IsComplete { get; set; }
}
