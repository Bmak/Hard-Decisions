using UnityEngine;

public class ScreamingHysteria : BaseEffect
{
    private string _key = "Screaming Hysteria";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: шанс 50% → на этот ход Perimeter+2 (требование по слоту Perimeter увеличить на 2)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            float success = Random.value;
            if (success <= 0.5f)
            {
                var buff = new FacilityBuff();
                buff.Charge = 2;

                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key, buff);

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
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Perimeter"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }
}
