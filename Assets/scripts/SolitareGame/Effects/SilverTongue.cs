public class SilverTongue : BaseEffect
{
    private string _key = "Silver Tongue";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: польза персонажа на этот ход = G
    //В любом слоте: Kitchen+1, этот персонаж на этот ход получает +SS(+2 ед.социальной пользы)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            int amount = 0;
            var buff = new CharBuff();
            foreach (ProfessionStat stat in card.Char.Def.Stats)
            {
                amount += stat.Amount;
                buff.AddCharStat(stat.Type, -stat.Amount);
            }
            buff.AddCharStat(ProfessionType.General, amount);

            S.CharacterService.AddCharBuff(card.Char, SolGamePhase.Draw.ToString() + _key, buff);

            EffectsManager.EndMoveAction += CancelDrawBuff;
        }

        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                var facBuff = new FacilityBuff();
                facBuff.Charge = 1;
                S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key, facBuff);

                var buff = new CharBuff();
                buff.AddCharStat(ProfessionType.Social, 2);
                S.CharacterService.AddCharBuff(card.Char, _key, buff);

                EffectsManager.EndMoveAction += CancelSetupBuff;
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Kitchen"), _key);

        //CancelSetupBuff()
        S.CharacterService.RemoveCharBuff(Card.Char, _key);
        EffectsManager.EndMoveAction -= CancelSetupBuff;

        OnComplete();
    }

    private void CancelDrawBuff()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, SolGamePhase.Draw.ToString()+_key);

        EffectsManager.EndMoveAction -= CancelDrawBuff;

        OnComplete();
    }

    private void CancelSetupBuff()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelSetupBuff;

        OnComplete();
    }
}
