public class Pattern : BaseEffect
{
    private string _key = "unused";
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
            
        }

        OnComplete();
    }

    public override void RevertEffect()
    {
        

        OnComplete();
    }

}
