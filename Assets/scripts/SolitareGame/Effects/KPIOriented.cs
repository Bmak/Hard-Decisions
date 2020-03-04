using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class KPIOriented : BaseEffect
{
    private string _key = "KPI Oriented";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В любом слоте: на этот ход → этот персонаж получает +GG (2 ед. общей пользы), Medical+1 (МЕД требование растет на 1)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup && card.Char.Slot != null)
        {
            var buff = new CharBuff();
            buff.AddCharStat(ProfessionType.General, 2);
            S.CharacterService.AddCharBuff(card.Char, _key, buff);

            FacilityBuff fbuff = new FacilityBuff();
            fbuff.Charge = 1;
            S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, fbuff);

            EffectsManager.EndMoveAction += CancelSetupEffect;
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

    private void CancelSetupEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

}
