using UnityEngine;

public class Short : BaseEffect
{
    private string _key = "Short";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //OnDraw: шанс 20% → эта карта превращается в "Dwarf" навсегда
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            float success = Random.value;
            if (success <= 0.6f)
            {
                string key = "Dwarf";
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
