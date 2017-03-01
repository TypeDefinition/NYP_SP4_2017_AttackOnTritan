using UnityEngine;
using System.Collections;

public class TouchScreenCameraController : MonoBehaviour {

	public Camera camera;
	private CameraMovement cameraMovement;
	[Range(0.0f, 100.0f)] [SerializeField]
	private float movementSpeed;
	private float forwardInput, rightInput, upInput;

	// Use this for initialization
	void Start () {
		if (camera != null) {
			cameraMovement = camera.gameObject.GetComponent<CameraMovement>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID == false
			gameObject.SetActive(false);
		#endif

		if (cameraMovement == null) {
			return;
		}

		float distance = movementSpeed * Time.deltaTime;
		cameraMovement.Move(distance * forwardInput, distance * rightInput, distance * upInput);
	}

	public float GetMovementSpeed() {
		return movementSpeed;
	}

	public Camera GetCamera() {
		return camera;
	}

	public void SetMovementInputAxis(float _forward, float _right) {
		forwardInput = Mathf.Clamp(_forward, -1.0f, 1.0f);
		rightInput = Mathf.Clamp(_right, -1.0f, 1.0f);
	}

	public void SetFlightInputAxis(float _up) {
		upInput = Mathf.Clamp(_up, -1.0f, 1.0f);
	}

	public void RotateCameraRight() {
		if (cameraMovement == null) {
			return;
		}
		cameraMovement.RotateRight();
	}

	public void RotateCameraLeft() {
		if (cameraMovement == null) {
			return;
		}
		cameraMovement.RotateLeft();
	}

	public void CycleCameraMode() {
		if (cameraMovement == null) {
			return;
		}
		cameraMovement.CycleMode();
	}

}