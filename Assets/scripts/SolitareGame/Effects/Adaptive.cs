using UnityEngine;

public class Adaptive : BaseEffect
{
    private string _key = "Adaptive";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: 20% шанс → эта карта превращается в "Changeling" навсегда
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            float chance = Random.value;
            if (chance <= 0.6f)
            {
                string key = "Changeling";
                var buff = new CardBuff();
                buff.Def = S.CharacterService.EffectCardsDict[key].DeepCopy();
                S.CharacterService.AddCardBuff(card, _key, buff);
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {


        OnComplete();
    }

}
