using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    GameCtrl gameCtrl;
	DeckCtrl deckCtrl;
	UnitManager unitCtrl;

	void Start () {
        gameCtrl = GetComponent<GameCtrl>();
        deckCtrl = GetComponent<DeckCtrl>();
		unitCtrl = GetComponent<UnitManager>();
	}

	public void AllocateNewCharsCards (List<CharController> newChars) {
		List<CardController> toDiscard = new List<CardController>();
		foreach (var nc in newChars) {
			toDiscard.AddRange(nc.myExistingCards);
		}

		unitCtrl.SendCharsToHold(newChars);
	    deckCtrl.SendCardsToDeck(toDiscard, true);

	}




}
