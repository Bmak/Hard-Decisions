using Random = UnityEngine.Random;

public class NewYorker : BaseEffect
{
    private string _key = "NewYorker";

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    private CardObject _card;
    //OnDraw: 80% шанс → эта карта превращается в "Mean" или "Workaholic" (равный шанс) на этот ход
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            float success = Random.value;
            if (success <= 0.6f)
            {
                float rnd = Random.value;

                string key = rnd >= 0.5f ? "Mean" : "Workaholic";

                var buff = new CardBuff();
                buff.Def = S.CharacterService.EffectCardsDict[key].DeepCopy();

                S.CharacterService.AddCardBuff(_card, _key, buff);

                EffectsManager.EndMoveAction += CancelDrawEffect;
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
        S.CharacterService.RemoveCardBuff(Card, _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

}
