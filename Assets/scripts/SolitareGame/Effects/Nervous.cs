
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

    //OnDraw: -1 MOR, 15% → эта карта превращается в "Paranoid" или "Unhinged" навсегда;
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            S.VitalService.AddBuff(VitalType.MOR, _key, -1);

            if (Random.value <= 0.20f)
            {
                string key = Random.value >= 0.5f ? "Paranoid" : "Unhinged";

                var buff = new CardBuff();
                buff.Def = S.CharacterService.EffectCardsDict[key].DeepCopy();

                S.CharacterService.AddCardBuff(_card, _key, buff);
            }
        }
        
        OnComplete();
    }

    public override void RevertEffect()
    {
        //S.VitalService.RemoveBuff(VitalType.MOR, _key);
        //S.CharacterService.RemoveCardBuff(_card, _key);

        OnComplete();
    }
}
