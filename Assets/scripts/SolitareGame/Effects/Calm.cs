public class Calm : BaseEffect
{
    private string _key = "Calm";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В любом слоте: этот персонаж получает на этот ход +1 ед. пользы для этого слота, -2 MOR
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                //S.VitalService.AddBuff(VitalType.MOR, _key, -2);

                var buff = new CharBuff();
                buff.AddCharStat(ProfessionType.General, 1);
                S.CharacterService.AddCharBuff(card.Char, _key, buff);

                EffectsManager.EndMoveAction += CancelSetupEffect;
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
