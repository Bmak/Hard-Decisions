using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public enum SolGamePhase
{
    StartReset = 0,
    Draw = 1,
    Report = 2,
    PreStart = 3,
    Setup = 4,
    Day = 5,
    Checkpoint = 6,
    Night = 7,
    End = 8 //срабатывают эффекты OnDiscard
}

public class SolitareGameControl : MonoBehaviour
{
    [SerializeField] private Text _dayText;
    [SerializeField] private Text _phaseText;
    [SerializeField] private Button _endDayButton;
    [SerializeField] private Image _lockerImage;

    private SolGamePhase _currentPhase = SolGamePhase.End;

    private SolGamePhase[] _waitForCommand = new SolGamePhase[] {SolGamePhase.Day};

    private int _roundCounter;

    private readonly List<SolitareObject> _registredObjects = new List<SolitareObject>();

    private void Start()
    {
        _endDayButton.onClick.AddListener(OnEndDay);
    }

    private void OnEndDay()
    {
        SetPhase(SolGamePhase.Day);
    }

    public void InitGame()
    {
        SetPhase(SolGamePhase.StartReset);
    }

    public void RegisterSolitareObject(SolitareObject obj)
    {
        _registredObjects.Add(obj);

        obj.OnComplete += OnPhaseComplete;
    }

    private void SetPhase(SolGamePhase phase)
    {
        if (_currentPhase == phase) return;

        Debug.Log("======= PHASE: " + phase + " =====================");

        if (phase == SolGamePhase.StartReset)
        {
            _roundCounter++;
            UpdateDay();
        }

        _currentPhase = phase;

        _phaseText.text = string.Format("Phase: {0}", _currentPhase.ToString());

        _registredObjects.ForEach(o => o.SetPhase(phase));

        _endDayButton.interactable = _currentPhase == SolGamePhase.Setup;
        _lockerImage.gameObject.SetActive(_currentPhase != SolGamePhase.Setup);

        if (phase == SolGamePhase.End)
        {
            EffectsManager.EndMoveAction();
        }
    }

    private void OnPhaseComplete()
    {
        if (IsPhaseComplete())
        {
            _registredObjects.ForEach(x => x.IsComplete = false);

            this.DelayedMethod(1f, NextPhase);
        }
    }

    private void NextPhase()
    {
        string[] values = Enum.GetNames(typeof(SolGamePhase));

        string currentPhase = _currentPhase.ToString();

        for (int i = 0; i < values.Length; i++)
        {
            string p = values[i];
            if (p == currentPhase)
            {
                int index = i + 1;
                SolGamePhase nextPhase = index < values.Length ? (SolGamePhase) index : 0;
                if (_waitForCommand.Any(x => x == nextPhase)) return;
                SetPhase(nextPhase);
                return;
            }
        }
    }

    private bool IsPhaseComplete()
    {
        foreach (SolitareObject solitareObject in _registredObjects)
        {
            if (!solitareObject.IsComplete) return false;
        }

        return true;
    }

    public void ClearGame()
    {
        _registredObjects.Clear();
        _roundCounter = 0;
        UpdateDay();
    }

    private void UpdateDay()
    {
        _dayText.text = "Day: " + _roundCounter;
    }
}
