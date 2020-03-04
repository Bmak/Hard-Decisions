using UnityEngine;

public class Unhinged : BaseEffect
{
    private string _key = "Unhinged";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В любом слоте: шанс 30% → на этот ход: Medical+2, Engineering+1 (требование по МЕД увел. на 2, а по ИНЖ увел. на 1)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                float chance = Random.value;
                Debug.Log(string.Format("Unhinged chance: {0}", chance <= 0.3f ? "TRUE" : "FALSE"));
                if (chance <= 0.3f)
                {
                    var mBuff = new FacilityBuff();
                    mBuff.Charge = 2;
                    S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, mBuff);

                    var eBuff = new FacilityBuff();
                    eBuff.Charge = 1;
                    S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Engineering"), _key, eBuff);

                    EffectsManager.EndMoveAction += RevertEffect;
                }
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Engineering"), _key);

        EffectsManager.EndMoveAction -= RevertEffect;

        OnComplete();
    }

}
