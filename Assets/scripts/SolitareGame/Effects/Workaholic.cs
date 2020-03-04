using System.Collections.Generic;

public class Workaholic : BaseEffect
{
    private string _key = "Workaholic";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //"OnDraw: польза персонажа на этот ход равна нулю
    //В слоте Hold: -3 MOR"
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            var buff = new CharBuff();
            foreach (ProfessionStat stat in card.Char.Def.Stats)
            {
                buff.AddCharStat(stat.Type, -stat.Amount);
            }
            S.CharacterService.AddCharBuff(card.Char, _key, buff);

            EffectsManager.EndMoveAction += CancelDrawEffect;
        }

        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                S.VitalService.RemoveBuff(VitalType.MOR, _key);
            }
            else
            {
                S.VitalService.AddBuff(VitalType.MOR, _key, -2);
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
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }
}
