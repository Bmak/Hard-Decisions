using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour {

	public Camera activeCam;
	public int minFOV = 40;
	public int maxFOV = 100;
	public float zoomFactor = 1f;
	public float moveFactor = 1f;
	public Vector3 moveLimits;
	Vector3 startPos;


	// Use this for initialization
	void Start () {
		startPos = activeCam.transform.position;
	}
	
	void SetFOV(float axisValue) {

		float zoomChange = axisValue * zoomFactor;
		float newZoom = activeCam.fieldOfView + zoomChange;

		if (zoomChange < 0f) {
			activeCam.fieldOfView = Mathf.Max(minFOV, newZoom);
		} else if (zoomChange > 0f) {
			activeCam.fieldOfView = Mathf.Min(newZoom, maxFOV);
		}
	}

	void MoveCam (Vector3 movement) {
		Vector3 curPos = activeCam.transform.position;
		Vector3 newPos = curPos + movement;

        if (Mathf.Abs(newPos.x - startPos.x) > moveLimits.x)
            newPos.x = startPos.x + moveLimits.x * Mathf.Sign(movement.x);
        if (Mathf.Abs(newPos.y - startPos.y) > moveLimits.y)
            newPos.y = startPos.y + moveLimits.y * Mathf.Sign(movement.y);
/*      if (Mathf.Abs(newPos.z - startPos.z) > moveLimits.z)
            newPos.z = startPos.z + moveLimits.z * Mathf.Sign(movement.z); */
        activeCam.transform.position = newPos;

    }

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
			SetFOV (Input.GetAxis("Mouse ScrollWheel"));
		}

		if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) {
			float moveX = Input.GetAxis("Horizontal") * moveFactor;
			float moveY = Input.GetAxis("Vertical") * moveFactor;
			Vector3 move = new Vector3(moveX, moveY, 0f);
			MoveCam(move);
		}
	}
}
