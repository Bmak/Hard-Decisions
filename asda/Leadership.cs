
using System.Collections.Generic;

public class Leadership : BaseEffect
{
    private string _key = "Leadership";
    private CardObject _card;

    public override string Key
    {
        get { return _key; }
    }

    protected override CardObject Card
    {
        get { return _card; }
    }

    private List<ProfessionStat> _oldStats = new List<ProfessionStat>();

    //OnDraw: польза персонажа на этот ход = AG (1 агрес. польза, 1 общая польза). В любом слоте: +3 MOR
    public override void SetEffect(SolGamePhase phase, CardObject card)
    {
        _card = card;
        if (phase == SolGamePhase.Draw)
        {
            _oldStats.AddRange(card.Char.Def.Stats);

            List<ProfessionStat> stats = new List<ProfessionStat>();
            ProfessionStat stat = new ProfessionStat();
            stat.Type = ProfessionType.Aggressive;
            stat.Amount = 1;

            stats.Add(stat);

            stat.Type = ProfessionType.General;
            stat.Amount = 1;

            stats.Add(stat);

            card.Char.Def.Stats = stats;

            EffectsManager.EndMoveAction += OnEndMove;
        }

        if (phase == SolGamePhase.Setup && card.Char.Slot != null)
        {
            //S.Vitals[VitalType.MOR] += 3;
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        //TODO

        OnComplete();
    }

    private void OnEndMove()
    {
        Card.Char.Def.Stats = _oldStats;

        EffectsManager.EndMoveAction -= OnEndMove;

        OnComplete();
    }
}
