
using UnityEngine;

public class Nervous : BaseEffect
{
    private string _key = "Nervous";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    private string _oldKey;
    //OnDraw: -1 MOR, 15% → эта карта превращается в "Paranoid" или "Unhinged" навсегда;
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        /*
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            S.Vitals[VitalType.MOR] -= 1;

            float chance = 0.15f;
            if (Random.value <= chance)
            {
                _oldKey = _card.Def.name;
                string key = Random.value >= 0.5f ? "Paranoid" : "Unhinged";

                _card.Def = S.EffectCardsDict[key];
            }
        }
        */
        OnComplete();
    }

    public override void RevertEffect()
    {
        S.Vitals[VitalType.MOR] += 1;

        Card.Def = S.EffectCardsDict[_oldKey];

        OnComplete();
    }
}
