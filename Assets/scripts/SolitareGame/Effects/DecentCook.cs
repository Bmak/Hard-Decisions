public class DecentCook : BaseEffect
{
    private string _key = "Decent Cook";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В слоте Kitchen: этот персонаж на этот ход получает +G (+1 ед. общей пользы)
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
                    buff.AddCharStat(ProfessionType.General, 2);

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
