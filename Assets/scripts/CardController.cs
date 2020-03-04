using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// This is for card UI control
public class CardController : MonoBehaviour {

    //[HideInInspector]
	public CardScrObj cardInfo = null;

    // CARD UI
    [Header("UI Parts")]
	public Text labelName;
    public Text labelDescription;
    public Image labelArtwork;

    public bool cardCanBeOpened = true;
    GameCtrl gameCtrl;

    void Start() {
        gameCtrl = FindObjectOfType<GameCtrl>();
        SetDraggable(false);
        // DisplayCard();
    }

    public void DisplayCard() {

        print(name + ": displaying " + gameObject.name + "...");

        if (cardInfo != null)
        {
            gameObject.name = "card_" + cardInfo.name;
            labelName.text = cardInfo.name;
            labelDescription.text = cardInfo.description;
            labelArtwork.color = Color.white;
            labelArtwork.sprite = cardInfo.artwork;
        }
        else
        {
            Debug.LogError(gameObject.name + ": cardInfo == null!");
        }

        print(name + ": " + gameObject.name + " displayed.");

    }

    public void OpenCard()
    {
        if (!cardCanBeOpened) return;

        var f = GetComponent<Flippable>();
        if (f.currentlyClosed)
        {
            f.Flip();
//            f.enabled = false;
        }

        gameCtrl.OnCardOpen(gameObject);
        cardCanBeOpened = false;

    }

    public void CloseCard() {
        //if (cardCanBeOpened) return;

        var f = GetComponent<Flippable>();
        if (!f.currentlyClosed)
        {
//            f.enabled = true;
            f.Flip();
        }

        cardCanBeOpened = true;
    }

    public void SetDraggable(bool value) {
        var d = GetComponent<Draggable>();
        d.enabled = value;
    }



}
