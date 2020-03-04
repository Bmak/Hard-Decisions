using UnityEngine;

public class Changeling : BaseEffect
{
    private string _key = "Changeling";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //В слоте Perimeter: этот персонаж получает +A (+1 ед. агр. пользы) на этот ход
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            if (Random.value <= 0.4f)
            {
                string key = "Polymorph";
                var buff = new CardBuff();
                buff.Def = S.CharacterService.EffectCardsDict[key].DeepCopy();
                S.CharacterService.AddCardBuff(card, _key, buff);
            }
            else
            {
                var buff = new CharBuff();
                buff.AddCharStat(ProfessionType.General, 1);
                S.CharacterService.AddCharBuff(card.Char, _key, buff);

                EffectsManager.EndMoveAction += CancelDrawEffect;
            }
        }

        OnComplete();
    }

    private void CancelDrawEffect()
    {
        S.CharacterService.RemoveCharBuff(Card.Char, _key);

        EffectsManager.EndMoveAction -= CancelDrawEffect;

        OnComplete();
    }

    public override void RevertEffect()
    {


        OnComplete();
    }

}
