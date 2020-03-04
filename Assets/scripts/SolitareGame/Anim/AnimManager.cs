
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class AnimManager : SolitareObject
{
    private readonly Queue<Action<Action>> _phaseAnimations = new Queue<Action<Action>>();
    private bool _isAnimating;

    private void Awake()
    {
        S.AnimManager = this;
    }

    public void AddAnim(Action<Action> animation)
    {
        _phaseAnimations.Enqueue(animation);
    }

    public override void SetPhase(SolGamePhase phase)
    {
        //if (_isAnimating) return;

        _isAnimating = true;
        NextAnim();
    }

    private void NextAnim()
    {
        if (_phaseAnimations.Count > 0)
        {
            Action<Action> animation = _phaseAnimations.Dequeue();
            animation.Invoke(NextAnim);

            return;
        }

        _isAnimating = false;
        IsComplete = true;
        OnComplete();
    }

    public override bool IsComplete { get; set; }
}
