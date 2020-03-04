
using System;

public abstract class BaseEffect
{
    public abstract string Key { get; }
    protected abstract CardObject Card { get; }

    public abstract void SetEffect(SolGamePhase phase, CardObject card);
    public abstract void RevertEffect();

    public Action OnComplete = () => { };
}
