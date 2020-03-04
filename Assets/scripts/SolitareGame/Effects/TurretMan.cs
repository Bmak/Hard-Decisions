using UnityEngine;

public class TurretMan : BaseEffect
{
    private string _key = "Turret Man";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //"OnDraw: все карты персонажа, кроме этой, уничтожаются; польза персонажа навсегда равна нулю
    //В слоте Perimeter: этот персонаж на этот ход получает +ААА(+3 ед.агрессивной пользы), +1 MOR"
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

            card.Char.Deck.Clear();
            card.Char.Deck.Add(card.Def);
            card.Char.Drop.Clear();
        }

        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                if (card.Char.Slot.Def.Name == "Perimeter")
                {
                    var buff = new CharBuff();

                    foreach (ProfessionStat stat in card.Char.Def.Stats)
                    {
                        buff.AddCharStat(stat.Type, -stat.Amount);
                    }

                    buff.AddCharStat(ProfessionType.Aggressive, 3);
                    S.CharacterService.AddCharBuff(card.Char, _key, buff);

                    int morBuff = 1;
                    S.VitalService.AddBuff(VitalType.MOR, _key, morBuff);

                    EffectsManager.EndMoveAction += CancelSetupEffect;
                }
            }
        }

        OnComplete();
    }

    private void CancelSetupEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);
        S.VitalService.RemoveBuff(VitalType.MOR, _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

}
