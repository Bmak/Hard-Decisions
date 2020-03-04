public class TightenTheBelt : BaseEffect
{
    private string _key = "Tighten The Belt";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: Kitchen−1 (требование по кухне снижается на 1)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            var buff = new FacilityBuff();
            buff.Charge = -1;
            S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key, buff);

            EffectsManager.EndMoveAction += CancelDrawEffect;
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        OnComplete();
    }

    private void CancelDrawEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

}
