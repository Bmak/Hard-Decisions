using UnityEngine;

public class Overwheight : BaseEffect
{
    private string _key = "Overwheight";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: на этот ход Kitchen+1 (треб. по кухне +1), шанс 15% → на этот ход Medical+1 (требование по МЕД +1)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            var buff = new FacilityBuff();
            buff.Charge = 1;

            S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key, buff);
        
            float success = Random.value;
            if (success <= 0.2f)
            {
                var mbuff = new FacilityBuff();
                mbuff.Charge = 1;

                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, mbuff);
            }

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
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }
}
