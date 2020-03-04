
using System.Collections.Generic;

public class Leadership : BaseEffect
{
    private string _key = "Leadership";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: польза персонажа на этот ход = AG (1 агрес. польза, 1 общая польза). В любом слоте: +3 MOR
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            var b = new CharBuff();

            foreach (ProfessionStat stat in card.Char.Def.Stats)
            {
                b.AddCharStat(stat.Type, -stat.Amount);
            }

            b.AddCharStat(ProfessionType.Aggressive, 1);
            b.AddCharStat(ProfessionType.General, 1);

            S.CharacterService.AddCharBuff(card.Char, _key, b);

            EffectsManager.EndMoveAction += CancelDrawEffect;
        }

        if (phase == SolGamePhase.Setup && card.Char.Slot != null)
        {
            S.VitalService.AddBuff(VitalType.MOR, _key, 3);
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.VitalService.RemoveBuff(VitalType.MOR, _key);

        OnComplete();
    }

    private void CancelDrawEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

}
