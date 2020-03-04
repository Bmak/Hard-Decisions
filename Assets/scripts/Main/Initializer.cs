using UnityEngine;

public class Initializer : MonoBehaviour
{
    private SolitareGameControl _solitareGameControl;
    private HoldController _hold;
    private SlotsController _slots;
    private AnimManager _animManager;

    private void Start()
    {
        _solitareGameControl = FindObjectOfType<SolitareGameControl>();
        _slots = FindObjectOfType<SlotsController>();
        _hold = FindObjectOfType<HoldController>();
        _animManager = FindObjectOfType<AnimManager>();

        _solitareGameControl.RegisterSolitareObject(_hold);
        _solitareGameControl.RegisterSolitareObject(_slots);
        _slots.Slots.ForEach(s => _solitareGameControl.RegisterSolitareObject(s));
        S.CharacterService.Chars.ForEach(c => _solitareGameControl.RegisterSolitareObject(c));
        _solitareGameControl.RegisterSolitareObject(_animManager);

        _solitareGameControl.InitGame();
    }

}
