using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropHandler))]
public class HoldController : SolitareObject
{
    [SerializeField] private CharCardView _cardPrefab;
    [SerializeField] private Transform _cardHolder;

    private void Awake()
    {
        if (S.CharacterService.Chars == null)
        {
            S.CharacterService.Chars = new List<CharCardView>();
        }
        S.CharacterService.Chars.Clear();

        _cardPrefab.gameObject.SetActive(true);
        foreach (CharacterScrObj character in S.CharacterService.CharacterDefs)
        {
            CharCardView card = Instantiate(_cardPrefab, _cardHolder);
            card.SetData(character.DeepCopy());
            S.CharacterService.Chars.Add(card);
        }
        _cardPrefab.gameObject.SetActive(false);
    }


    public override void SetPhase(SolGamePhase phase)
    {
        if (phase == SolGamePhase.StartReset)
        {
            S.CharacterService.Chars.ForEach(x => x.transform.SetParent(_cardHolder));
        }

        IsComplete = true;
        OnComplete();
    }

    public override bool IsComplete { get; set; }
}
