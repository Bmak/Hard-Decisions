using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// This is for char UI control
public class CharController : MonoBehaviour
{
    //[HideInInspector]
    public CharacterScrObj charInfo = null;
    public List<CardController> myExistingCards;
    public bool infected;

    // CARD UI
    [Header("UI parts")]
    public Text labelName;
    public Text labelDescription;
    public Image labelArtwork;

    bool cardCanBeOpened = true;
    GameCtrl gameCtrl;

    void Start() {
        gameCtrl = GameObject.FindObjectOfType<GameCtrl>();
        SetDraggable(false);
        // DisplayCharacter();
    }

    public void DisplayCard() {

        print(name + ": displaying " + gameObject.name + "...");

        if (charInfo != null) {
            gameObject.name = "char_" + charInfo.firstName;
            labelName.text = charInfo.firstName + " " + charInfo.lastName;
            labelDescription.text = charInfo.description;
            labelArtwork.color = Color.white;
            labelArtwork.sprite = charInfo.artwork;
            infected = charInfo.infected; 
        } else {
            Debug.LogError(gameObject.name + ": charInfo == null!");
        }

        print(name + ": " + gameObject.name + " displayed.");

    }

    public void OpenCharacterCard() {
        if (!cardCanBeOpened) return;

        var f = GetComponent<Flippable>();
        if (f.currentlyClosed) {
            f.Flip();
            f.enabled = false;
        }

        gameCtrl.OnCharCardOpen(gameObject);
        cardCanBeOpened = false;

    }

    public void SetDraggable(bool value) {
        var d = GetComponent<Draggable>();
        d.enabled = value;
    }


}
