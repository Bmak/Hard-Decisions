public class StreetBully : BaseEffect
{
    private string _key = "Street Bully";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В любом слоте: -2 MOR, Medical+1 на этот ход, Perimeter+1 на этот ход
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                //S.VitalService.AddBuff(VitalType.MOR, _key, -2);

                var mBuff = new FacilityBuff();
                mBuff.Charge = 1;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, mBuff);

                var pBuff = new FacilityBuff();
                pBuff.Charge = 1;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key, pBuff);

                EffectsManager.EndMoveAction += CancelSetupEffect;
            }
        }

        OnComplete();
    }

    private void CancelSetupEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.VitalService.RemoveBuff(VitalType.MOR, _key);

        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

}
