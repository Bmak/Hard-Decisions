using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class KPIOriented : BaseEffect
{
    private string _key = "KPI Oriented";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    //'В любом слоте: этот персонаж получает +G (1 ед. общей пользы), Medical+1 (МЕД требование растет на 1 на этот ход)
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Setup && card.Char.Slot != null)
        {
            card.Char.Def.ChangeCharStat(ProfessionType.General, 1);
            EffectsManager.ChangeAnySlotStat(ProfessionType.Medical, 1);
            EffectsManager.EndMoveAction += RevertEffect;
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        Card.Char.Def.ChangeCharStat(ProfessionType.General, -1);
        EffectsManager.ChangeAnySlotStat(ProfessionType.Medical, -1);
        EffectsManager.EndMoveAction -= RevertEffect;

        OnComplete();
    }

}
