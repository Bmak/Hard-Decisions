using UnityEngine;

public class Dwarf : BaseEffect
{
    private string _key = "Dwarf";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В слоте Perimeter: этот персонаж получает +A (+1 ед. агр. пользы) на этот ход
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            if (Random.value <= 0.4f)
            {
                string key = "Turret Man";
                var buff = new CardBuff();
                buff.Def = S.CharacterService.EffectCardsDict[key].DeepCopy();
                S.CharacterService.AddCardBuff(card, _key, buff);
            }
            else
            {
                S.VitalService.AddBuff(VitalType.MOR, _key, -2);

                var buff = new FacilityBuff();
                buff.Charge = 1;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, buff);

                EffectsManager.EndMoveAction += CancelDrawEffect;
            }
        }

        OnComplete();
    }

    private void CancelDrawEffect()
    {
        S.VitalService.RemoveBuff(VitalType.MOR, _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

    public override void RevertEffect()
    {


        OnComplete();
    }

}
