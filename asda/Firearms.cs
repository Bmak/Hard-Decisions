
using System.Linq;

public class Firearms : BaseEffect
{
    private string _key = "Firearms";
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
        if (phase == SolGamePhase.Setup)
        {
            if (card.Char.Slot != null)
            {
                if (card.Char.Slot.Def.Name == "Perimeter")
                {
                    card.Char.Def.ChangeCharStat(ProfessionType.Aggressive, 1);

                    EffectsManager.EndMoveAction += RevertEffect;
                }
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        Card.Char.Def.ChangeCharStat(ProfessionType.Aggressive, -1);
        EffectsManager.EndMoveAction -= RevertEffect;

        OnComplete();
    }

}
