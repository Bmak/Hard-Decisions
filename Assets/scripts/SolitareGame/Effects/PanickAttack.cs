public class PanickAttack : BaseEffect
{
    private string _key = "Panick Attack";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //"OnDraw: польза персонажа на этот ход равна нулю, Medical+3 (требование слота МЕД увел. на 3), -3 MOR
    //(""Breathing is a full time job"")"
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

            S.VitalService.AddBuff(VitalType.MOR, _key, -3);

            var fbuff = new FacilityBuff();
            fbuff.Charge = 3;
            S.FacilityService.AddFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key, fbuff);

            EffectsManager.EndMoveAction += CancelDrawEffect;
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
        S.FacilityService.RemoveFacilityBuff(S.FacilityService.GetSlotById("Medical"), _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }
}
