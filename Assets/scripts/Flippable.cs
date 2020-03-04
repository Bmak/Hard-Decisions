using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flippable : MonoBehaviour {

    public bool startsClosed = true;
    //[HideInInspector]
    public bool currentlyClosed = true;

    public GameObject face;
    public GameObject back;

	void Start () {
        currentlyClosed = startsClosed;

        if (!startsClosed) {
            gameObject.BroadcastMessage("DisplayCard");
        }
        if (back.activeInHierarchy != startsClosed) Flip();
	}

    public void Flip() {
        currentlyClosed = !currentlyClosed;

        face.SetActive(!currentlyClosed);
        back.SetActive(currentlyClosed);

    } 

}
