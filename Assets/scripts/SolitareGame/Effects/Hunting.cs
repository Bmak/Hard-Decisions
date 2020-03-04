public class Hunting : BaseEffect
{
    private string _key = "Hunting";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //"В слоте Kitchen: этот персонаж получает на этот ход +G (+1 ед. общей пользы)
    //В слоте Perimeter: этот персонаж получает на этот ход +A(+1 ед.агрессивной пользы)"
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                if (card.Char.Slot.Def.Id == "Kitchen")
                {
                    var buff = new CharBuff();
                    buff.AddCharStat(ProfessionType.General, 1);
                    S.CharacterService.AddCharBuff(card.Char, _key, buff);

                    EffectsManager.EndMoveAction += RevertEffect;
                }
                if (card.Char.Slot.Def.Id == "Perimeter")
                {
                    var buff = new CharBuff();
                    buff.AddCharStat(ProfessionType.Aggressive, 1);
                    S.CharacterService.AddCharBuff(card.Char, _key, buff);

                    EffectsManager.EndMoveAction += RevertEffect;
                }
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= RevertEffect;

        OnComplete();
    }

}
