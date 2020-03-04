public class VeganCook : BaseEffect
{
    private string _key = "Vegan Cook";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В слоте Kitchen: этот персонаж получает на этот ход +G (1 ед. общей пользы); -1 MOR
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                if (card.Char.Slot.Def.Id == "Kitchen")
                {
                    S.VitalService.AddBuff(VitalType.MOR, _key, -1);

                    var buff = new CharBuff();
                    buff.AddCharStat(ProfessionType.General, 2);
                    S.CharacterService.AddCharBuff(card.Char, _key, buff);

                    EffectsManager.EndMoveAction += CancelSetupEffect;
                }
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.VitalService.RemoveBuff(VitalType.MOR, _key);
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

    private void CancelSetupEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelSetupEffect;

        OnComplete();
    }

}
