using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera camera;
	[Range(0.0f, 100.0f)] [SerializeField]
	private float movementSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		CameraMovement cameraMovement = null;
		if (camera == null || camera.GetComponent<CameraMovement>() == null) {
			return;
		} else {
			cameraMovement = camera.GetComponent<CameraMovement>();
		}

		if (Input.GetButtonDown("Change Camera Mode")) {
			cameraMovement.CycleMode();
		}

		if (Input.GetButtonDown("Camera Rotate Left")) {
			cameraMovement.RotateLeft();
		} else if (Input.GetButtonDown("Camera Rotate Right")) {
			cameraMovement.RotateRight();
		}

		cameraMovement.Move(Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"), Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"), Time.deltaTime * movementSpeed * Input.GetAxis("Camera Height"));
	}

	public float GetMovementSpeed() {
		return movementSpeed;
	}

	public Camera GetCamera() {
		return camera;
	}

}