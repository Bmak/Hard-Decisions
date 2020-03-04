public class PTSD : BaseEffect
{
    private string _key = "PTSD";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: -1 MOR
    //В слоте Perimeter: → OnDiscard: перед сбросом эта карта превращается в "Panic Attack" навсегда
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            S.VitalService.AddBuff(VitalType.MOR, _key, -1);
        }

        if (phase == SolGamePhase.Setup) {
            if (card.Char.Slot != null) {
                if (card.Char.Slot.Def.Id == "Perimeter") {

                    var buff = new CharBuff();
                    buff.AddCharStat(ProfessionType.Aggressive, 2);

                    S.CharacterService.AddCharBuff(card.Char, _key, buff);

                    EffectsManager.EndMoveAction += RevertEffect;
                }
            }
        }

        if (phase == SolGamePhase.End)
        {
            if (card.Char.Slot != null)
            {
                if (card.Char.Slot.Def.Id == "Perimeter")
                {
                    var buff = new CardBuff();
                    string key = "Panick Attack";
                    buff.Def = S.CharacterService.EffectCardsDict[key].DeepCopy();
                    S.CharacterService.AddCardBuff(card, _key, buff);
                }
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {

        OnComplete();
    }

}
