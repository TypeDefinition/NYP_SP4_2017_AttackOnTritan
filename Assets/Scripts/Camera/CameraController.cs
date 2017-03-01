using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	CameraMovement cameraMovement;

	// Use this for initialization
	void Start () {
		cameraMovement = gameObject.GetComponent<CameraMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraMovement == null) {
			cameraMovement = gameObject.GetComponent<CameraMovement>();
			if (cameraMovement == null) {
				return;
			}
		}

		if (Input.GetButtonDown("Change Camera Mode")) {
			cameraMovement.CycleMode();
		}

		if (Input.GetButtonDown("Camera Rotate Left")) {
			cameraMovement.RotateLeft();
		} else if (Input.GetButtonDown("Camera Rotate Right")) {
			cameraMovement.RotateRight();
		}

		cameraMovement.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), Input.GetAxis("Camera Height"));
	}
}
