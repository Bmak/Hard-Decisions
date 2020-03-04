using Random = UnityEngine.Random;

public class NewYorker : BaseEffect
{
    private string _key = "New Yorker";

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    private string _oldKey;
    private CardObject _card;
    //OnDraw: 80% шанс → эта карта превращается в "Mean" или "Workaholic" (равный шанс) на этот ход
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            float chance = 0.8f;

            float success = Random.value;
            if (success <= chance)
            {
                float rnd = Random.value;

                _oldKey = card.Def.name;
                string key = rnd >= 0.5f ? "Mean" : "Workaholic";

                card.Def = S.EffectCardsDict[key];

                EffectsManager.EndMoveAction += RevertEffect;
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        Card.Def = S.EffectCardsDict[_oldKey];

        EffectsManager.EndMoveAction -= RevertEffect;

        OnComplete();
    }

}
