using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCtrl : MonoBehaviour {

	public List<CardController> drawPile;
	public List<CardController> discardPile;
	public List<CardController> hand;

	public Transform drawCardholder;
	public Transform discardCardholder;
	public Transform handCardholder;

	void Start () {}

	public void SendCardsToDeck(List<CardController> cards, bool toDiscard = true) {
		if (cards == null) return;

		if (toDiscard) {
			discardPile.AddRange(cards);
			foreach (var card in cards) {
				card.CloseCard();
				card.SetDraggable(false);
				card.transform.SetParent(discardCardholder);
			}
		} else {
            drawPile.AddRange(cards);
            foreach (var card in cards) {
                card.CloseCard();
                card.SetDraggable(false);
                card.transform.SetParent(drawCardholder);
            }
        }

	}

	public void DrawCardsToHand (int cardsAmount) {
		for (int c = 1; c <= cardsAmount; c++) {
			DrawTopCard();
		}		
	}

	void DrawTopCard () {
		if (drawPile.Count <= 0) {
			if (discardPile.Count <= 0) {
				Debug.LogWarning("Both Draw and Discard piles are empty!");
				return;
			} else {
				ShuffleDiscardToDraw();
			}
		}

        var card = drawPile[drawPile.Count - 1];
        hand.Add(card);
        card.transform.SetParent(handCardholder);
		card.SetDraggable(true);
		card.GetComponent<Flippable>().Flip();

        drawPile.RemoveAt(drawPile.Count - 1);
    }

	public void ShuffleDiscardToDraw() {
		ProBro.ShuffleArray(discardPile.ToArray());
		drawPile.AddRange(discardPile);

		for (int i = 0; i < drawPile.Count; i++) {
			var card = drawPile[i];
			card.transform.SetParent(drawCardholder);
			card.transform.SetSiblingIndex(i);
		}

		discardPile.Clear();
	}

}
