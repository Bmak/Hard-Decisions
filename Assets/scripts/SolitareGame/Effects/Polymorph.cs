public class Polymorph : BaseEffect
{
    private string _key = "Polymorph";
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
            var buff = new CharBuff();

            int amount = 3;
        /*  foreach (ProfessionStat stat in card.Char.Def.Stats)
            {
                buff.AddCharStat(stat.Type, -stat.Amount);
                amount += stat.Amount;
            } */

            for (int i = 0; i < amount; i++)
            {
                buff.AddCharStat(ProfessionStat.GetRandomType(ProfessionType.None), 1);
            }
        }

        OnComplete();
    }

    public override void RevertEffect()
    {


        OnComplete();
    }

}
