using UnityEngine;

public class Paranoid : BaseEffect
{
    private string _key = "Paranoid";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: -3 MOR; шанс 35% → на этот ход: Medical+2, Perimeter+1 (треб. по МЕД +2, по периметру +1)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            S.VitalService.AddBuff(VitalType.MOR, _key, -2);

            if (Random.value <= 0.35f)
            {
                var mBuff = new FacilityBuff();
                mBuff.Charge = 2;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, mBuff);

                var pBuff = new FacilityBuff();
                pBuff.Charge = 1;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key, pBuff);

                EffectsManager.EndMoveAction += CancelDrawEffect;
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        OnComplete();
    }

    private void CancelDrawEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

}
