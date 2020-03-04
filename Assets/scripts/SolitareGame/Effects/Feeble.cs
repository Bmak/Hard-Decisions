using UnityEngine;

public class Feeble : BaseEffect
{
    private string _key = "Feeble";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: на этот ход Medical+1 (требование по МЕД +1); Шанс 20% → добавить 1 копию этой карты в discard pile этого персонажа навсегда
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            var buff = new FacilityBuff();
            buff.Charge = 1;
            S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, buff);

            EffectsManager.EndMoveAction += CancelDrawEffect;

            float chance = Random.value;
            if (chance <= 0.2f)
            {
                CardScrObj def = S.CharacterService.EffectCardsDict[card.Def.name].DeepCopy();
                card.Char.Drop.Add(def);
                Debug.Log("Add Card to Drop: " + def.name);
            }
        }

        OnComplete();
    }

    private void CancelDrawEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

    public override void RevertEffect()
    {


        OnComplete();
    }

}
