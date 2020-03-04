public class MasterNetworking : BaseEffect
{
    private string _key = "Master Networking";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В любом слоте: Kitchen+1 (треб. по кухне +1) на этот ход, +3 MOR
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                S.VitalService.AddBuff(VitalType.MOR, _key, 3);

                var buff = new FacilityBuff();
                buff.Charge = 1;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key, buff);

                EffectsManager.EndMoveAction += CancelSetupEffect;
            }
        }

        OnComplete();
    }

    private void CancelSetupEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.VitalService.RemoveBuff(VitalType.MOR, _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

}
